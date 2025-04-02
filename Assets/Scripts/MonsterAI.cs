using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public enum State { Patrol, Rage, Chase, Attack, LookAround }

    [Header("Main Settings")]
    public Transform player;
    public Transform[] patrolPoints;
    
    [Header("Movement Settings")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float rotationSpeed = 10f;
    public float attackRange = 1.5f;
    
    [Header("Detection Settings")]
    public float detectionRange = 15f;
    public float fieldOfView = 90f;
    public LayerMask obstacleLayers;
    
    [Header("Timers")]
    public float rageDuration = 2f;
    public float chaseDuration = 10f;
    public float lookAroundDuration = 3f;
    public float patrolWaitTime = 1f;

    private NavMeshAgent agent;
    private MonsterSound sound;
    private MonsterAnimation anim;
    private State currentState;
    private int currentPatrolIndex;
    private Vector3 lastKnownPlayerPos;
    private float stateTimer;
    private float patrolTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sound = GetComponent<MonsterSound>();
        anim = GetComponent<MonsterAnimation>();
        
        agent.speed = patrolSpeed;
        currentState = State.Patrol;
    }

    private void Update()
    {
        UpdateStateMachine();
    }

    private void UpdateStateMachine()
    {
        switch(currentState)
        {
            case State.Patrol: UpdatePatrol(); break;
            case State.Rage: UpdateRage(); break;
            case State.Chase: UpdateChase(); break;
            case State.Attack: UpdateAttack(); break;
            case State.LookAround: UpdateLookAround(); break;
        }
    }

    private void UpdatePatrol()
    {
        if(patrolPoints.Length == 0) return;
        
        if(agent.remainingDistance < 0.5f)
        {
            patrolTimer += Time.deltaTime;
            
            if(patrolTimer >= patrolWaitTime)
            {
                patrolTimer = 0;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            }
        }
        
        if(CanSeePlayer())
        {
            lastKnownPlayerPos = player.position;
            ChangeState(State.Rage);
        }
    }

    private void UpdateRage()
    {
        FaceTarget(player.position);
        
        if(stateTimer <= 0)
        {
            stateTimer = rageDuration;
            agent.isStopped = true;
            sound.isStopped = true;
            anim.PlayRage();
            sound.PlayRage();
        }
        
        stateTimer -= Time.deltaTime;
        
        if(stateTimer <= 0)
        {
            ChangeState(State.Chase);
        }
    }

    private void UpdateChase()
    {
        agent.isStopped = false;
        sound.isStopped = false;
        agent.speed = chaseSpeed;
        agent.SetDestination(lastKnownPlayerPos);
        
        anim.SetChasing(true);
        sound.PlayChase();
        
        if(Vector3.Distance(transform.position, player.position) < attackRange)
        {
            ChangeState(State.Attack);
            return;
        }
        
        if(CanSeePlayer())
        {
            lastKnownPlayerPos = player.position;
            stateTimer = chaseDuration;
        }
        else
        {
            stateTimer -= Time.deltaTime;
            if(stateTimer <= 0 || agent.remainingDistance < 0.5f)
            {
                ChangeState(State.LookAround);
            }
        }
    }

    private void UpdateAttack()
    {
        agent.isStopped = true;
        sound.isStopped = true;
        FaceTarget(player.position);
        
        if(stateTimer <= 0)
        {
            stateTimer = 1f;
            anim.PlayAttack();
            sound.PlayAttack();
        }
        
        stateTimer -= Time.deltaTime;
        
        if(stateTimer <= 0)
        {
            if(Vector3.Distance(transform.position, player.position) > attackRange * 1.2f)
            {
                ChangeState(State.Chase);
            }
            else
            {
                // Убийство игрока
                Debug.Log("Player killed!");
                ChangeState(State.Patrol);
            }
        }
    }

    private void UpdateLookAround()
    {
        agent.isStopped = true;
        sound.isStopped = true;
        anim.SetMoving(false);
        
        if(stateTimer <= 0)
        {
            stateTimer = lookAroundDuration;
            sound.PlayBreathe();
        }
        
        transform.Rotate(0, 90 * Time.deltaTime, 0);
        stateTimer -= Time.deltaTime;
        
        if(stateTimer <= 0)
        {
            ChangeState(State.Patrol);
        }
    }

    private void ChangeState(State newState)
    {
        switch(currentState)
        {
            case State.Rage: anim.ResetRage(); break;
            case State.Attack: anim.ResetAttack(); break;
            case State.Chase: anim.SetChasing(false); break;
        }
        
        currentState = newState;
        agent.isStopped = false;
        sound.isStopped = false;
        
        switch(newState)
        {
            case State.Patrol:
                agent.speed = patrolSpeed;
                anim.SetMoving(true);
                sound.SetFootstepSpeed(false);
                break;
                
            case State.Chase:
                stateTimer = chaseDuration;
                sound.SetFootstepSpeed(true);
                break;
        }
    }

    private bool CanSeePlayer()
    {
        if(player == null) return false;
        
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;
        
        if(distance > detectionRange) return false;
        
        float angle = Vector3.Angle(transform.forward, direction);
        if(angle > fieldOfView / 2) return false;
        
        return !Physics.Raycast(transform.position, direction, distance, obstacleLayers);
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}