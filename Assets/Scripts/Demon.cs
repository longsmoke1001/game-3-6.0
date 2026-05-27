using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Demon : EnemyAttacker
{
    float lastAttackTime = -9999f;
    [SerializeField] float attackDelay = 0.4f;
    [SerializeField] GameObject fireball;
    [SerializeField] GameObject ring;
    float firingAngle;
    float lastRingTime = -9999f;
    float ringTime = 1f;
    float ringOffset = 10f;
    float ringCooldown = 5f;
    [SerializeField] float ringRadius = 3.42f;
    float ringNum = 10;
    float ringDelay = 1f;
    float lastTeleportTime = -5f;
    float teleportCooldown = 10f;
    float teleportRange = 5f;
    [SerializeField] GameObject circle;
    public enum EnemyState
    {
        Idle,
        Attacking,
        Casting,
        Teleporting,
    }
    public EnemyState state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
        anim.speed = 0.5f; // Slow down the animation speed to 50%
    }
    protected override void Update()
    {
        base.Update();
        switch (state)
        {
            case EnemyState.Idle:
                if (Time.time - lastRingTime > ringCooldown)
                    Cast();
                else if (Time.time - lastTeleportTime > teleportCooldown)
                    Teleport();
                else if ((player.transform.position - transform.position).magnitude < attackRange && Time.time - lastAttackTime > 1 / attackSpeed)
                    Attack();
                break;
            case EnemyState.Attacking:
                HandleAttack();
                break;
            case EnemyState.Casting:
                HandleCasting();
                break;
            case EnemyState.Teleporting:
                HandleTeleporting();
                break;
        }
    }

    void HandleTeleporting()
    {
        if (Time.time - lastTeleportTime > attackDelay)
        {
            Vector2 playerPoisition = player.transform.position;
            float xMin = Mathf.Max(playerPoisition.x - teleportRange, -9);
            float xMax = Mathf.Min(playerPoisition.x + teleportRange, 19);
            float yMin = Mathf.Max(playerPoisition.y - teleportRange, -9);
            float yMax = Mathf.Min(playerPoisition.y + teleportRange, 9);
            Vector2 newPos = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
            transform.position = newPos;
            state = EnemyState.Idle;
        }
    }
    void Teleport()
    {
        state = EnemyState.Teleporting;
        lastTeleportTime = Time.time;
    }
    void HandleCasting()
    {
        if (Time.time - lastAttackTime > attackDelay)
        {
            for (int i = 0; i < ringNum; i++)
                StartCoroutine("CreateRing");
            state = EnemyState.Idle;
        }
    }
    void Cast()
    {
        state = EnemyState.Casting;
        lastRingTime = Time.time;
        if (anim != null)
            anim.SetTrigger("2_Attack");
    }
    protected void HandleAttack()
    {
        if (Time.time - lastAttackTime > attackDelay)
        {
            lastAttackTime = Time.time;
            state = EnemyState.Idle;
            GameObject f = Instantiate(fireball, transform.position, Quaternion.Euler(0, 0, firingAngle));
            f.GetComponent<Fire>().damage = attackPower;
            f.GetComponent<Fire>().source = this;
            f.SetActive(true);
            Destroy(f,1);
        }
    }

    IEnumerator CreateRing()
    {
        Vector3 offset = new Vector3(Random.Range(-ringOffset, ringOffset), Random.Range(-ringOffset, ringOffset), 0);
        Vector3 ringPos = transform.position + offset;
        GameObject c = Instantiate(circle, ringPos, Quaternion.identity);
        c.transform.localScale *= ringRadius / 1.75f;
        Destroy(c,ringDelay);
        yield return new WaitForSeconds(ringDelay);
        GameObject r = Instantiate(ring, ringPos, Quaternion.identity);
        r.transform.localScale *= ringRadius/1.75f;
        Destroy(r, ringTime);
        r.GetComponent<Fire>().damage = attackPower;
        r.GetComponent<Fire>().source = this;
        r.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            r.GetComponent<CircleCollider2D>().radius = ringRadius/1.75f /6*(i+1)/5;
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }
    protected void Attack()
    {
        lastAttackTime = Time.time;
        Vector2 toPlayer = player.transform.position - transform.position;
        firingAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        state = EnemyState.Attacking;
        if (anim != null)
            anim.SetTrigger("2_Attack");

    }
}
