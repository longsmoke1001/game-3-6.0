using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField] Animator anim;
    [SerializeField] GameManager gameManager;
    float lastDashTime = -9999f;
    float lastAttackTime = -9999f;
    float lastDefendTime = -9999f;
    [SerializeField] Slider healthBar;
    [SerializeField] GameObject shieldEffect;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] Joystick joystick;
    [SerializeField] Button attackButton;
    [SerializeField] Button defendButton;
    bool attackButtonPressed = false;
    bool defendButtonPressed = false;

    [field: SerializeField] public PlayerData playerData { get; private set; }
    public static Player Instance { get; private set; }

    public enum PlayerState
    {
        Idle,
        Moving,
        Dashing,
        Attacking,
        Defending
    }

    public PlayerState state = PlayerState.Idle;
    // Start is called before the first frame update
    void Start()
    {
        attackButton.onClick.AddListener(AttackButton);
        defendButton.onClick.AddListener(DefendButton);
        maxHealth = playerData.maxHealth;
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case PlayerState.Idle:
                TryMove();
                TryDash();
                TryAttack();
                TryDefend();
                break;
            case PlayerState.Moving:
                TryStop();
                TryDash();
                TryAttack();
                HandleMoving();
                TryDefend();
                break;
            case PlayerState.Dashing:
                HandleDashing();
                break;
            case PlayerState.Attacking:
                if (playerData.canMoveWhileAttacking)
                    HandleMoving();
                HandleAttacking();
                break;
            case PlayerState.Defending:
                if (playerData.canMoveWhileDefending)
                    HandleMoving();
                HandleDefending();
                break;
        }
        if (Input.GetKeyDown(KeyCode.D) || joystick.Horizontal > 0)
            transform.localScale = new Vector2(-1, 1);
        if (Input.GetKeyDown(KeyCode.A) || joystick.Horizontal < 0)
            transform.localScale = new Vector2(1, 1);
        if (currHealth <= 0)
        {
            gameManager.Losing();
        }
    }

    void TryStop()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            state = PlayerState.Idle;
            anim.SetBool("1_Move", false);
        }
    }
    void TryMove()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            state = PlayerState.Moving;
            anim.SetBool("1_Move", true);
        }
    }
    void HandleAttacking()
    {
        if (Time.time - lastAttackTime > playerData.attackdelay)
        {
            foreach (var e in gameManager.Enemies)
                if ((e.transform.position - transform.position).magnitude < playerData.attackRange)
                {
                    Debug.Log("attack");
                    currHealth = Mathf.Min(currHealth + playerData.healthOnHit, maxHealth);
                    e.TakeDamage(playerData.attackPower, this);
                    ParticleSystem h = Instantiate(hitEffect, e.transform.position, Quaternion.identity);
                    Destroy(h.gameObject, 0.1f);
                }
            state = PlayerState.Idle;
        }
    }
    void HandleDashing()
    {
        transform.Translate(((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized * Time.deltaTime * playerData.dashSpeed);
        if (Time.time - lastDashTime > playerData.dashDuration)
        {
            state = PlayerState.Idle;
            anim.SetBool("1_Move", false);
            GetComponent<Collider2D>().isTrigger = false;
        }
    }
    void TryDash()
    {
        //if (Input.GetMouseButtonDown(1) && Time.time - lastDashTime > dashCooldown)
        //{
        //    state = PlayerState.Dashing;
        //    anim.SetBool("1_Move", true);
        //    GetComponent<Collider2D>().isTrigger = true;
        //    transform.localScale = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).x > 0 ? new Vector2(-1, 1) : new Vector2(1, 1);
        //    lastDashTime = Time.time;
        //}
    }
    void HandleMoving()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector2.up * Time.deltaTime * playerData.movSpeed);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector2.down * Time.deltaTime * playerData.movSpeed);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector2.left * Time.deltaTime * playerData.movSpeed);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector2.right * Time.deltaTime * playerData.movSpeed);
        transform.Translate(joystick.Horizontal * Time.deltaTime * playerData.movSpeed, joystick.Vertical * Time.deltaTime * playerData.movSpeed, 0);
    }
    void TryAttack()
    {
        if ((Input.GetMouseButton(0) || attackButtonPressed) && Time.time - lastAttackTime > 1 / playerData.attackSpeed)
        {
            attackButtonPressed = false;
            lastAttackTime = Time.time;
            anim.SetTrigger("2_Attack");
            state = PlayerState.Attacking;
        }
    }
    void TryDefend()
    {
        if ((Input.GetMouseButton(1) || defendButtonPressed) && Time.time - lastDefendTime > playerData.defendCooldown)
        {
            defendButtonPressed = false;
            state = PlayerState.Defending;
            lastDefendTime = Time.time;
            shieldEffect.SetActive(true);
        }
    }
    void HandleDefending()
    {
        anim.SetBool("1_Move", false);
        if (Time.time - lastDefendTime > playerData.defendDuration)
        {
            state = PlayerState.Idle;
            shieldEffect.SetActive(false);
        }
    }

    public override void TakeDamage(float amount, Character source)
    {
        if (state == PlayerState.Defending)
        {
            Debug.Log("defend");
            if (playerData != null && this != null) source.TakeDamage(playerData.attackPower * playerData.reflectedDamageMultiplier, this);
            currHealth = Mathf.Min(currHealth + playerData.defendHealMultiplier * playerData.attackPower * playerData.reflectedDamageMultiplier, maxHealth);
            return;
        }
        base.TakeDamage(amount, source);
        anim.SetTrigger("3_Damaged");
    }
    public void AttackButton()
    {
        if (state == PlayerState.Idle || state == PlayerState.Moving)
            attackButtonPressed = true;
    }

    public void DefendButton()
    {
        if (state == PlayerState.Idle || state == PlayerState.Moving)
            defendButtonPressed = true;
    }
}
