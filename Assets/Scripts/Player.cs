using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public PlayerData runtimePlayerData;
    [SerializeField] protected Joystick joystick;
    [SerializeField] protected Animator anim;
    [SerializeField] protected GameManager gameManager;
    protected float lastAttackTime = -9999f;
    protected bool attackButtonPressed = false;
    protected bool defendButtonPressed = false;

    protected override void Start()
    {
        joystick = FindAnyObjectByType<Joystick>();
        runtimePlayerData = GlobalGameManager.Instance.runtimePlayerData;
        maxHealth = runtimePlayerData.maxHealth;
        currHealth = maxHealth;
        gameManager= FindAnyObjectByType<GameManager>();
    }
    public enum PlayerState
    {
        Idle,
        Moving,
        Attacking,
        Casting,
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) || joystick.Horizontal > 0)
            transform.localScale = new Vector2(-1, 1);
        if (Input.GetKeyDown(KeyCode.A) || joystick.Horizontal < 0)
            transform.localScale = new Vector2(1, 1);
        if (currHealth <= 0)
        {
            gameManager.Losing();
        }
    }
    public PlayerState state = PlayerState.Idle;
    protected void HandleMoving()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector2.up * Time.deltaTime * runtimePlayerData.movSpeed);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector2.down * Time.deltaTime * runtimePlayerData.movSpeed);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector2.left * Time.deltaTime * runtimePlayerData.movSpeed);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector2.right * Time.deltaTime * runtimePlayerData.movSpeed);
        transform.Translate(joystick.Horizontal * Time.deltaTime * runtimePlayerData.movSpeed, joystick.Vertical * Time.deltaTime * runtimePlayerData.movSpeed, 0);
    }

    protected void TryAttack()
    {
        if ((Input.GetMouseButton(0) || attackButtonPressed) && Time.time - lastAttackTime > 1 / runtimePlayerData.attackSpeed)
        {
            attackButtonPressed = false;
            lastAttackTime = Time.time;
            anim.SetTrigger("2_Attack");
            state = PlayerState.Attacking;
        }
    }
    protected void TryMove()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            state = PlayerState.Moving;
            anim.SetBool("1_Move", true);
        }
    }
    protected void TryStop()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            state = PlayerState.Idle;
            anim.SetBool("1_Move", false);
        }
    }
}
