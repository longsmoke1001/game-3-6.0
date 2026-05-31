using UnityEngine;

public class Ranger : Player
{
    [SerializeField] Projectile arrow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        switch (state)
        {
            case PlayerState.Idle:
                TryMove();
                TryAttack();
                TryCast();
                break;
            case PlayerState.Moving:
                HandleMoving();
                TryAttack();
                TryStop();
                break;
            case PlayerState.Attacking:
                HandleAttacking();
                break;
            case PlayerState.Casting:
                HandleCasting();
                break;
        }
    }

    void HandleAttacking()
    {
        if (Time.time - lastAttackTime > playerData.attackdelay)
        {
            float distance = 9999f;
            Enemy closestEnemy = null;
            foreach (var e in gameManager.Enemies)
            {
                float d = Vector2.Distance(transform.position, e.transform.position);
                if (d < distance)
                {
                    distance = d;
                    closestEnemy = e;
                }
            }
            Vector2 toEnemy = closestEnemy.transform.position - transform.position;
            Projectile a = Instantiate(arrow, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(toEnemy.y, toEnemy.x) * Mathf.Rad2Deg));
            a.damage = playerData.attackPower;
            state = PlayerState.Idle;
        }
    }

    void TryCast()
    {
        if (Input.GetMouseButton(1) || defendButtonPressed)
        {
            defendButtonPressed = false;
            anim.SetTrigger("3_Cast");
            state = PlayerState.Casting;
        }
    }

    void HandleCasting()
        {
            if (Time.time - lastDefendTime > playerData.defendCooldown)
            {
                foreach (var e in gameManager.Enemies)
                    if ((e.transform.position - transform.position).magnitude < playerData.defendRange)
                    {
                        e.TakeDamage(playerData.defendPower, this);
                        ParticleSystem h = Instantiate(hitEffect, e.transform.position, Quaternion.identity);
                        Destroy(h.gameObject, 0.1f);
                    }
                state = PlayerState.Idle;
            }
    }
}
