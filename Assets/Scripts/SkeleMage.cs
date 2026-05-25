using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SkeleMage : Skeleton
{
    [SerializeField] private GameObject attackEffect;
    [SerializeField] float correctionFactor = 1.75f;
    override protected void HandleAttack()
    {
        if (Time.time - lastAttackTime > attackDelay)
        {
            player.TakeDamage(attackPower, this);
            Vector2 direction = player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject beam=Instantiate(attackEffect, transform.position/2+player.transform.position/2 , Quaternion.Euler(0,0,angle));
            beam.transform.localScale = new Vector3(direction.magnitude*correctionFactor, 1, 1);
            Destroy(beam, 0.05f);
            state = EnemyState.Chasing;
        }
    }
}
