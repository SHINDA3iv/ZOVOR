using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    private Animator anim;
    
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int Chasing = Animator.StringToHash("Chasing");
    private static readonly int Rage = Animator.StringToHash("Rage");
    private static readonly int Attack = Animator.StringToHash("Attack");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetMoving(bool value)
    {
        anim.SetBool(Moving, value);
    }

    public void SetChasing(bool value)
    {
        anim.SetBool(Chasing, value);
    }

    public void PlayRage()
    {
        anim.SetTrigger(Rage);
    }

    public void ResetRage()
    {
        anim.ResetTrigger(Rage);
    }

    public void PlayAttack()
    {
        anim.SetTrigger(Attack);
    }

    public void ResetAttack()
    {
        anim.ResetTrigger(Attack);
    }

    public void OnAttackHit()
    {
        
    }
}