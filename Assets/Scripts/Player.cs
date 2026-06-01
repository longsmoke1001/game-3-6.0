using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public PlayerData runtimePlayerData;
    protected Joystick joystick;
    protected Animator anim;
    protected GameManager gameManager;
    public Sprite attackCooldownImage;
    public Sprite skillCooldownImage;
    protected CanvasManager canvasManager;
    protected Button attackButton;
    protected Button skillButton;
    protected float lastAttackTime = -9999f;
    protected float lastSkillTime = -9999f;
    protected bool attackButtonPressed = false;
    protected bool skillButtonPressed = false;
    public float attackButtonFill = 0f;
    public float skillButtonFill = 0f;

    protected override void Start()
    {
        base.Start();
        canvasManager = FindAnyObjectByType<CanvasManager>();
        gameManager = FindAnyObjectByType<GameManager>();
        anim = GetComponentInChildren<Animator>();
        joystick = canvasManager.joystick;
        attackButton = canvasManager.attackButton;
        skillButton = canvasManager.skillButton;
        //if (attackCooldownImage!=null)
        //    canvasManager.attackCooldownImage = attackCooldownImage;
        //if (skillCooldownImage!=null)
        //    canvasManager.skillCooldownImage = skillCooldownImage;
        runtimePlayerData = GlobalGameManager.Instance.runtimePlayerData;
        maxHealth = runtimePlayerData.maxHealth;
        currHealth = maxHealth;
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
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)|| joystick.Horizontal > 0)
            transform.localScale = new Vector2(-1, 1);
        if (Input.GetKeyDown(KeyCode.A) ||Input.GetKeyDown(KeyCode.LeftArrow)|| joystick.Horizontal < 0)
            transform.localScale = new Vector2(1, 1);
        if (currHealth <= 0)
        {
            gameManager.Losing();
        }
    }
    public PlayerState state = PlayerState.Idle;
    protected void HandleMoving()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            transform.Translate(Vector2.up * Time.deltaTime * runtimePlayerData.movSpeed);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            transform.Translate(Vector2.down * Time.deltaTime * runtimePlayerData.movSpeed);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(Vector2.left * Time.deltaTime * runtimePlayerData.movSpeed);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            transform.Translate(Vector2.right * Time.deltaTime * runtimePlayerData.movSpeed);
        transform.Translate(joystick.Horizontal * Time.deltaTime * runtimePlayerData.movSpeed, joystick.Vertical * Time.deltaTime * runtimePlayerData.movSpeed, 0);
    }
    protected void AttackButton()
    {
        if ((state == PlayerState.Idle || state == PlayerState.Moving) && Time.time - lastAttackTime > 1 / runtimePlayerData.attackSpeed)
        {
            attackButtonPressed = true;
            attackButton.interactable = false;
        }
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
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || joystick.Horizontal != 0 || joystick.Vertical != 0 || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            state = PlayerState.Moving;
            anim.SetBool("1_Move", true);
        }
    }
    protected void TryStop()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && joystick.Horizontal == 0 && joystick.Vertical == 0 && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            state = PlayerState.Idle;
            anim.SetBool("1_Move", false);
        }
    }
    virtual protected void SkillButton()
    {
        if ((state == PlayerState.Idle || state == PlayerState.Moving) && Time.time - lastSkillTime > runtimePlayerData.skillCooldown)
        {
            skillButtonPressed = true;
            skillButton.interactable = false;
        }
    }
}
