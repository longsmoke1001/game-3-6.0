using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [field: SerializeField] public PlayerData playerData { get; private set; }
    [SerializeField] protected Joystick joystick;
    [SerializeField] protected Animator anim;
    protected float lastAttackTime = -9999f;
    protected bool attackButtonPressed = false;
    protected bool defendButtonPressed = false;
    public enum PlayerState
    {
        Idle,
        Moving,
        Attacking,
        Casting,
    }
        public PlayerState state = PlayerState.Idle;
    protected void HandleMoving()
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

    protected void TryAttack()
    {
        if ((Input.GetMouseButton(0) || attackButtonPressed) && Time.time - lastAttackTime > 1 / playerData.attackSpeed)
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
