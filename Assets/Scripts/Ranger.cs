using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ranger : Player, IAttackable
{
    [SerializeField] Projectile arrow;
    [SerializeField] Projectile skill;
    [SerializeField] TextMeshProUGUI stackText;
    [SerializeField] float textOffsetY = 0.56f;
    [SerializeField] float textOffsetX = -1.79f;


    float lastCastTime = -9999f;
    int stacks = 0;

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
                break;
            case PlayerState.Casting:
                HandleCasting();
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
            for (int i = 0; i < runtimePlayerData.projectileCount; i++)
            {
                Projectile a = Instantiate(arrow, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(toEnemy.y, toEnemy.x)* Mathf.Rad2Deg + 15 * (runtimePlayerData.projectileCount-1) - 30 * i));
                if (runtimePlayerData.projReturn)
                    a.projReturn = true;
                if (runtimePlayerData.pierce)
                    a.projPierce = true;
                a.speed = runtimePlayerData.projSpeed;
                a.source = this;
                a.damage = runtimePlayerData.attackPower* (runtimePlayerData.movSpeedScaleDamage ? runtimePlayerData.movSpeed / 5 : 1);
            }
            state = PlayerState.Idle;
        }
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
            for (int i = 0; i < runtimePlayerData.projectileCount; i++)
            {
                Projectile a = Instantiate(skill, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(toEnemy.y, toEnemy.x) * Mathf.Rad2Deg + 15 * (runtimePlayerData.projectileCount - 1) - 30 * i));
                if (runtimePlayerData.projReturn)
                    a.projReturn = true;
                if (runtimePlayerData.pierce)
                    a.projPierce = true;
                a.speed = runtimePlayerData.projSpeed;
                a.damage = runtimePlayerData.attackPower * (1+stacks)*(runtimePlayerData.movSpeedScaleDamage?runtimePlayerData.movSpeed/5:1);
                a.source = this;
            }
            stacks = 0;
            state = PlayerState.Idle;
        }
    }
    public void Attack(Character target)
    {
        target.TakeDamage(runtimePlayerData.attackPower* (runtimePlayerData.movSpeedScaleDamage ? runtimePlayerData.movSpeed / 5 : 1), this);
        stacks++;
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
