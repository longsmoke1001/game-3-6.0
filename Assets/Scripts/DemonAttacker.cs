using System.Threading.Tasks;
using UnityEngine;

public class DemonAttacker : EnemyAttacker
{
    float attackCooldown = 1f;
    float lastAttackTime = -9999f;
    float attackTime = 0.75f;
    float damagingRange = 3f;
    Vector3 toPlayer;
    [SerializeField] GameObject circle;
    float lastSpinTime = -5f;
    float spinCooldown = 10f;
    float spinDuration = 5f;
    float spinAnimationTime=0.2f;
    float spinningSpeed = 20f;
    float rotatingSpeed = 150f;
    float currentAngle=0f;
    [SerializeField] Tornado tornado;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    override protected void Start()
    {
        base.Start();
        anim.speed = 0.2f / attackTime;
    }
    public enum EnemyState
    {
        Idle,
        Attacking,
        Spinning,
    }

    EnemyState state = EnemyState.Idle;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        switch (state)
        {
            case EnemyState.Idle:
                if (Time.time - lastSpinTime > spinCooldown)
                    Spin();
                else if (Time.time - lastAttackTime > attackCooldown)
                {
                    lastAttackTime = Time.time;
                    Debug.Log("Attacking");
                    Attack();
                }
                break;
            case EnemyState.Attacking:
                HandleAttack();
                break;
            case EnemyState.Spinning:
                HandleSpinning();
                break;
        }
    }

    void HandleSpinning()
    {
        float process = Time.time - lastSpinTime;
        float angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - currentAngle% 360;
        currentAngle +=angle<180&&angle>0||angle<-180?Time.deltaTime*rotatingSpeed:-Time.deltaTime * rotatingSpeed;
        if (currentAngle > 180)
            currentAngle -= 360;
        else if (currentAngle < -180)
            currentAngle += 360;
        transform.Translate(new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0)*spinningSpeed*Time.deltaTime,Space.World);
        if (process < spinAnimationTime)
            transform.rotation = Quaternion.Euler(0, 90*process / spinAnimationTime, 0);
        else if (process > spinAnimationTime && process < spinDuration - spinAnimationTime)
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (process < spinDuration && process > spinDuration - spinAnimationTime)
            transform.rotation = Quaternion.Euler(0, 90*(1-((process - spinDuration + spinAnimationTime) / spinAnimationTime)), 0);
        else if (process > spinDuration)
        {
            state = EnemyState.Idle;
        }
    }
    void Spin()
    {
        state = EnemyState.Spinning;
        Tornado t = Instantiate(tornado, transform.position, Quaternion.Euler(0, 90, 0), transform);
        t.damage = attackPower;
        t.source = this;
        Destroy(t.gameObject, spinDuration);
        currentAngle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        lastSpinTime = Time.time;
    }
    void Attack()
    {
        anim.SetTrigger("2_Attack");
        state = EnemyState.Attacking;
        toPlayer = player.transform.position - transform.position;
        GameObject c = Instantiate(circle, player.transform.position, Quaternion.identity);
        c.transform.localScale = new Vector3(damagingRange * 2, damagingRange * 2, 1);
        Destroy(c, attackTime);
    }

    void HandleAttack()
    {
        transform.Translate(toPlayer * Time.deltaTime / attackTime);
        if (Time.time - lastAttackTime > attackTime)
        {
            if (((Vector2)(player.transform.position - transform.position)).magnitude < damagingRange)
                player.TakeDamage(attackPower, this);
            state = EnemyState.Idle;
        }
    }
}
