using UnityEngine;

public class Ranger : Player
{
    [SerializeField] Projectile arrow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case PlayerState.Idle:
                TryMove();
                TryAttack();
                break;
            case PlayerState.Moving:
                HandleMoving();
                TryAttack();
                TryStop();
                break;
            case PlayerState.Attacking:
                HandleAttacking();
                break;
        }
    }

    void HandleAttacking()
    {
            if (Time.time - lastAttackTime > playerData.attackdelay)
            {
                Instantiate(arrow, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg));
            state = PlayerState.Idle;
            }
    }
}
