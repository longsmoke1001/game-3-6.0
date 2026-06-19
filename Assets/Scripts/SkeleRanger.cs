using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class SkeleRanger : Skeleton
{
    [SerializeField] Projectile projectilePrefab;
    protected override void HandleAttack()
    {
        if (Time.time - lastAttackTime > attackDelay)
        {
            Shoot(2, player, attackPower);
            state = EnemyState.Patrolling;
        }
    }
}
