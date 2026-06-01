using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Knight : Player
{
    [SerializeField] GameObject shieldEffect;
    [SerializeField] ParticleSystem hitEffect;
    Transform shieldEffectTransform;
    public static Player Instance { get; private set; }
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        shieldEffectTransform = GameObject.Find("InstantiateCanvas").transform;
        attackButton = canvasManager.attackButton;
        attackButton.onClick.AddListener(AttackButton);
        skillButton = canvasManager.skillButton;
        skillButton.onClick.AddListener(SkillButton);
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        switch (state)
        {
            case PlayerState.Idle:
                TryMove();
                TryAttack();
                TryDefend();
                break;
            case PlayerState.Moving:
                TryStop();
                TryAttack();
                HandleMoving();
                TryDefend();
                break;
            case PlayerState.Attacking:
                if (runtimePlayerData.canMoveWhileAttacking)
                    HandleMoving();
                HandleAttacking();
                break;
            case PlayerState.Casting:
                if (runtimePlayerData.canMoveWhileDefending)
                    HandleMoving();
                HandleDefending();
                break;
        }
        attackButtonFill = Mathf.Clamp01((Time.time - lastAttackTime) * runtimePlayerData.attackSpeed);
        skillButtonFill = Mathf.Clamp01((Time.time - lastSkillTime) / runtimePlayerData.skillCooldown);
        if ((Time.time - lastAttackTime) * runtimePlayerData.attackSpeed > 1)
            attackButton.interactable = true;
        if ((Time.time - lastSkillTime) / runtimePlayerData.skillCooldown > 1)
            skillButton.interactable = true;
    }
    void HandleAttacking()
    {
        if (Time.time - lastAttackTime > runtimePlayerData.attackdelay)
        {
            foreach (var e in gameManager.Enemies)
                if ((e.transform.position - transform.position).magnitude < runtimePlayerData.attackRange)
                {
                    Debug.Log("attack");
                    currHealth = Mathf.Min(currHealth + runtimePlayerData.healthOnHit, maxHealth);
                    e.TakeDamage(runtimePlayerData.attackPower, this);
                    ParticleSystem h = Instantiate(hitEffect, e.transform.position, Quaternion.identity);
                    Destroy(h.gameObject, 0.1f);
                }
            state = PlayerState.Idle;
        }
    }
    void TryDefend()
    {
        if ((Input.GetMouseButton(1) || skillButtonPressed) && Time.time - lastSkillTime > runtimePlayerData.skillCooldown)
        {
            skillButtonPressed = false;
            state = PlayerState.Casting;
            lastSkillTime = Time.time;
            Destroy(Instantiate(shieldEffect, transform.position, Quaternion.identity, shieldEffectTransform), runtimePlayerData.defendDuration);
        }
    }
    void HandleDefending()
    {
        anim.SetBool("1_Move", false);
        if (Time.time - lastSkillTime > runtimePlayerData.defendDuration)
            state = PlayerState.Idle;
    }

    public override void TakeDamage(float amount, Character source)
    {
        if (state == PlayerState.Casting)
        {
            Debug.Log("defend");
            if (runtimePlayerData != null && this != null) source.TakeDamage(runtimePlayerData.attackPower * runtimePlayerData.reflectedDamageMultiplier, this);
            currHealth = Mathf.Min(currHealth + runtimePlayerData.defendHealMultiplier * runtimePlayerData.attackPower * runtimePlayerData.reflectedDamageMultiplier, maxHealth);
            return;
        }
        base.TakeDamage(amount, source);
        anim.SetTrigger("3_Damaged");
    }
}
