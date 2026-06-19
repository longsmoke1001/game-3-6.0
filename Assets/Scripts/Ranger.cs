using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ranger : Player
{
    [SerializeField] Projectile arrow;
    [SerializeField] Projectile skill;
    [SerializeField] TextMeshProUGUI stackText;
    [SerializeField] float textOffsetY = 0.56f;
    [SerializeField] float textOffsetX = -1.79f;


    float lastCastTime = -9999f;
    public int stacks = 0;

    protected override void Start()
    {
        base.Start();
        Transform parent = GameObject.Find("InstantiateCanvas").transform;
        stackText=Instantiate(stackText, parent);
        attackButton = canvasManager.attackButton;
        attackButton.onClick.AddListener(AttackButton);
        skillButton = canvasManager.skillButton;
        skillButton.onClick.AddListener(SkillButton);
    }
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
                TryCast();
                TryStop();
                break;
            case PlayerState.Attacking:
                HandleAttacking();
                if (runtimePlayerData.canMoveWhileAttacking)
                    HandleMoving();
                break;
            case PlayerState.Casting:
                HandleCasting();
                if (runtimePlayerData.canMoveWhileAttacking)
                    HandleMoving();
                break;
        }
        stackText.gameObject.transform.position= Camera.main.WorldToScreenPoint(transform.position + new Vector3(textOffsetX,textOffsetY,0));
        stackText.text = stacks.ToString();
        attackButtonFill = Mathf.Clamp01((Time.time - lastAttackTime) * runtimePlayerData.attackSpeed);
        skillButtonFill = Mathf.Clamp01((Time.time - lastCastTime) * runtimePlayerData.attackSpeed);
        if ((Time.time - lastAttackTime) * runtimePlayerData.attackSpeed > 1)
            attackButton.interactable = true;
        if ((Time.time - lastCastTime) / runtimePlayerData.skillCooldown > 1)
            skillButton.interactable = true;
    }

    void HandleAttacking()
    {
        if (Time.time - lastAttackTime > runtimePlayerData.attackdelay)
        {
           ShootClosestEnemy(0,1);
            state = PlayerState.Idle;
        }
    }

    void ShootClosestEnemy(int projId,int attackRatio)
    {
        float distance = 9999f;
        Enemy closestEnemy = null;
        if (gameManager.Enemies.Count() == 0)
        {
            state = PlayerState.Idle;
            return;
        }
        foreach (var e in gameManager.Enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < distance)
            {
                distance = d;
                closestEnemy = e;
            }
        }
        Shoot(projId, closestEnemy, runtimePlayerData.attackPower * attackRatio*(runtimePlayerData.movSpeedScaleDamage ? runtimePlayerData.movSpeed / 5 : 1), runtimePlayerData.projSpeed, runtimePlayerData.projectileCount, projectileSpreadAngle, runtimePlayerData.projReturn, runtimePlayerData.pierce);
    }


    void TryCast()
    {
        if ((Input.GetMouseButton(1) || skillButtonPressed) && Time.time - lastCastTime > 1 / runtimePlayerData.attackSpeed)
        {
            skillButtonPressed = false;
            lastCastTime = Time.time;
            anim.SetTrigger("2_Attack");
            state = PlayerState.Casting;
        }
    }

    void HandleCasting()
    {
        if (Time.time - lastCastTime > runtimePlayerData.attackdelay)
        {
            ShootClosestEnemy(1,stacks+1);
            stacks=0;
            state = PlayerState.Idle;
        }
    }

    override protected void SkillButton()
    {
        if ((state == PlayerState.Idle || state == PlayerState.Moving) && (Time.time - lastSkillTime)*runtimePlayerData.attackSpeed > 1)
        {
            skillButtonPressed = true;
            skillButton.interactable = false;
        }
    }
}
