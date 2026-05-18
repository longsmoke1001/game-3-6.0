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
            Vector2 direction = player.transform.position - transform.position;
            Debug.Log(direction.y + "|" + direction.x);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Projectile proj = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle));
            proj.damage = attackPower;
            proj.source = this;
            state = EnemyState.Patrolling;
        }
    }
}
