using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform[] patrolPoints; // Точки патрулирования
    public Transform player;
    public float detectionRange = 20f; // Дистанция обнаружения
    public float attackRange = .3f; // Дистанция атаки
    public float fieldOfView = 60f; // Угол обзора
    public float chaseDuration = 5f; // Время преследования после потери видимости
    public float lookAroundDuration = 3f; // Время осматривания
    public Material highlightMaterial; // Материал для подсветки точек
    public Color visionConeColor = new Color(1, 0, 0, 0.3f); // Цвет зоны видимости

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool isChasing = false;
    private bool isLookingAround = false;
    private float chaseTimer = 0f;
    private float lookAroundTimer = 0f;
    private Material originalMaterial;
    private Renderer pointRenderer;
    private Vector3 lastKnownPlayerPosition;
    private bool hasLastKnownPosition = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Debug.Log("[Начало] ИИ монстра инициализирован.");
        PatrolToNextPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("[Атака] Игрок в зоне атаки!");
            KillPlayer();
            return;
        }

        if (CanSeePlayer())
        {
            Debug.Log("[Обнаружение] Игрок обнаружен!");
            HighlightPoint(player.position);
            agent.SetDestination(player.position);
            isChasing = true;
            chaseTimer = chaseDuration;
            lastKnownPlayerPosition = player.position;
            hasLastKnownPosition = true;
        }
        else if (isChasing)
        {
            chaseTimer -= Time.deltaTime;
            Debug.Log($"[Преследование] Игрок скрыт, продолжаем преследование еще {chaseTimer:F2} секунд.");

            if (hasLastKnownPosition)
            {
                agent.SetDestination(lastKnownPlayerPosition);
            }

            if (chaseTimer <= 0)
            {
                Debug.Log("[Поиск] Потерян контакт с игроком. Начинаем осмотр.");
                isChasing = false;
                isLookingAround = true;
                lookAroundTimer = lookAroundDuration;
                agent.isStopped = true;
                hasLastKnownPosition = false;
            }
        }
        else if (isLookingAround)
        {
            LookAround();
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isChasing && !isLookingAround)
        {
            Debug.Log("[Патруль] Достигнута точка патруля, переходим к следующей.");
            PatrolToNextPoint();
        }
    }

    void PatrolToNextPoint()
    {
        if (patrolPoints.Length == 0) return;
        Debug.Log($"[Патруль] Перемещение к точке патруля: {patrolPoints[currentPatrolIndex].name}");
        HighlightPoint(patrolPoints[currentPatrolIndex].position);
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle < fieldOfView / 2 && Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
            {
                Debug.DrawRay(transform.position, directionToPlayer * detectionRange, Color.red);
                if (hit.transform == player)
                {
                    Debug.Log("[Видимость] Игрок в поле зрения и линия видимости чиста.");
                    return true;
                }
            }
        }

        Debug.DrawRay(transform.position, directionToPlayer * detectionRange, Color.green);
        return false;
    }

    void LookAround()
    {
        lookAroundTimer -= Time.deltaTime;
        Debug.Log($"[Осмотр] Осматриваемся... Осталось {lookAroundTimer:F2} секунд.");
        transform.Rotate(0, 60 * Time.deltaTime, 0);

        if (lookAroundTimer <= 0)
        {
            Debug.Log("[Осмотр] Осмотр завершен, продолжаем патруль.");
            isLookingAround = false;
            agent.isStopped = false;
            PatrolToNextPoint();
        }
    }

    void KillPlayer()
    {
        Debug.Log("[Атака] Игрок мертв!");
    }

    void HighlightPoint(Vector3 position)
    {
        if (pointRenderer != null)
        {
            pointRenderer.material = originalMaterial;
        }

        Collider[] colliders = Physics.OverlapSphere(position, 0.5f);
        foreach (var col in colliders)
        {
            Renderer renderer = col.GetComponent<Renderer>();
            if (renderer != null)
            {
                Debug.Log("[Подсветка] Подсвечиваем точку.");
                originalMaterial = renderer.material;
                renderer.material = highlightMaterial;
                pointRenderer = renderer;
                break;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = visionConeColor;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Vector3 rightLimit = Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward * detectionRange;
            Vector3 leftLimit = Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward * detectionRange;

            Gizmos.DrawLine(transform.position, transform.position + rightLimit);
            Gizmos.DrawLine(transform.position, transform.position + leftLimit);
        }
    }
}
