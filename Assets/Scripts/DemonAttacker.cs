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
    float lastSpinTime = -9999f;
    float spinCooldown = 5f;
    float spinDuration = 2f;
    float spinAnimationTime=0.2f;
    float localScaleX=5f;
    [SerializeField] GameObject tornado;
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
        if (Time.time-lastSpinTime<spinAnimationTime)
            transform.localScale = new Vector3(localScaleX*(1-Time.time/spinAnimationTime), 0,0);
        else if (Time.time-lastSpinTime<spinDuration&&Time.time-lastSpinTime>spinDuration-spinAnimationTime)
            transform.localScale = new Vector3(localScaleX *((lastSpinTime+spinDuration-Time.time)/spinAnimationTime), 0,0);
        else if (Time.time - lastSpinTime > spinDuration)
        {
            state = EnemyState.Idle;
        }
    }
    void Spin()
    {
        state = EnemyState.Spinning;
        Destroy(Instantiate(tornado, transform.position,Quaternion.identity,transform),spinDuration);
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
