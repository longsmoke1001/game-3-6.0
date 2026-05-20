using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : EnemyAttacker
{
    protected float lastAttackTime = -9999f;
    [SerializeField] protected float attackDelay = 0.2f;

    // Update is called once per frame
    protected EnemyState state;
    public enum EnemyState
    {
        Patrolling,
        Attacking,
        Chasing,
    }
    protected override void Update()
    {
        base.Update();
        switch (state)
        {
            case EnemyState.Patrolling:
                Patrol();
                TryChase();
                break;
            case EnemyState.Chasing:
                HandleChasing();
                TryPatrol();
                TryAttack();
                break;
            case EnemyState.Attacking:
                HandleAttack();
                break;
        }

    }

    protected void TryPatrol()
    {
        if (!CanSeePlayer())
            state = EnemyState.Patrolling;
    }
    protected void TryChase()
    {
        if (CanSeePlayer())
            state = EnemyState.Chasing;
    }
    protected void HandleChasing()
    {
        isWaiting = false;
        Vector2 toPlayer = (Vector2)playerTransform.position - (Vector2)transform.position;
        if (toPlayer.magnitude > attackRange-0.5f)
        {
            transform.Translate(toPlayer.normalized * movSpeed * Time.deltaTime);
            if (anim != null)
                anim.SetBool("1_Move", true);
        }
        else if (anim != null)
                anim.SetBool("1_Move", false);

    }

    protected virtual void TryAttack()
    {
        if ((player.transform.position - transform.position).magnitude < attackRange && Time.time - lastAttackTime > 1 / attackSpeed)
        {
            anim.SetTrigger("2_Attack");
            state = EnemyState.Attacking;
            lastAttackTime = Time.time;
        }

    }

    protected virtual void HandleAttack()
    {
        if (Time.time - lastAttackTime > attackDelay)
        {
            player.TakeDamage(attackPower, this);
            state = EnemyState.Chasing;
        }
    }
}
