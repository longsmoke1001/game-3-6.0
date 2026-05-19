using System.Threading.Tasks;
using UnityEngine;

public class Demon : Skeleton
{
    [SerializeField] GameObject fireball;
    [SerializeField] GameObject ring;
    float fireTime = 0.5f;
    float firingAngle;
    float lastRingTime = -9999f;
    float ringTime = 1f;
    float ringCooldown = 5f;
    float ringRadius = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
        anim.speed = 0.5f; // Slow down the animation speed to 50%
    }

    // Update is called once per frame
    //protected override void Update()
    //{
    //    switch (state)
    //    {
    //        case EnemyState.Patrolling:
    //            Patrol();
    //            TryAttack();
    //            break;
    //        case EnemyState.Chasing:
    //            break;
    //        case EnemyState.Attacking:
    //            HandleAttack();
    //            break;
    //    }
    //}

    override protected void HandleAttack()
    {
        if (Time.time - lastAttackTime > attackDelay)
        {
            lastAttackTime = Time.time;
            state = EnemyState.Patrolling;
            if (Time.time - lastRingTime > ringCooldown)
            {
                lastRingTime = Time.time;
                GameObject r = Instantiate(ring, transform.position, Quaternion.identity);
                r.GetComponent<Fire>().damage = attackPower;
                r.GetComponent<Fire>().source = this;
                r.SetActive(true);
                Ring(r);
            }
            else
            {
                GameObject f = Instantiate(fireball, transform.position, Quaternion.Euler(0, 0, firingAngle));
                f.GetComponent<Fire>().damage = attackPower;
                f.GetComponent<Fire>().source = this;
                f.SetActive(true);
                Fireball(f);
            }
        }
    }

    async Task Ring(GameObject r)
    {
        for (int i = 0; i < 5; i++)
        {
            await Task.Delay((int)(ringTime * 200));
            r.GetComponent<CircleCollider2D>().radius = ringRadius * i;
        }
        Destroy(r);
    }
    async Task Fireball(GameObject f)
    {
        await Task.Delay((int)(fireTime * 1000));
        Destroy(f);
    }
    override protected void TryAttack()
    {
        if ((player.transform.position - transform.position).magnitude < attackRange && Time.time - lastAttackTime > 1 / attackSpeed)
        {
            lastAttackTime = Time.time;
            Vector2 toPlayer = player.transform.position - transform.position;
            firingAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
            state = EnemyState.Attacking;
            if (anim != null)
                anim.SetTrigger("2_Attack");
        }
    }
}
