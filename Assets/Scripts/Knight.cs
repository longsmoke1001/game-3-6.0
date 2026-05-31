using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Knight : Player
{

    [SerializeField] GameManager gameManager;
    float lastDefendTime = -9999f;
    [SerializeField] Slider healthBar;
    [SerializeField] GameObject shieldEffect;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] Button attackButton;
    [SerializeField] Button defendButton;
    [SerializeField] Image attackCooldownImage;
    [SerializeField] Image defendCooldownImage;


    public static Player Instance { get; private set; }



    public PlayerState state = PlayerState.Idle;
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        attackButton.onClick.AddListener(AttackButton);
        defendButton.onClick.AddListener(DefendButton);
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
                if (playerData.canMoveWhileAttacking)
                    HandleMoving();
                HandleAttacking();
                break;
            case PlayerState.Casting:
                if (playerData.canMoveWhileDefending)
                    HandleMoving();
                HandleDefending();
                break;
        }
        attackCooldownImage.fillAmount = Mathf.Clamp01((Time.time - lastAttackTime) * playerData.attackSpeed);
        defendCooldownImage.fillAmount = Mathf.Clamp01((Time.time - lastDefendTime) / playerData.defendCooldown);
        if ((Time.time - lastAttackTime) * playerData.attackSpeed > 1)
            attackButton.interactable = true;
        if ((Time.time - lastDefendTime) / playerData.defendCooldown > 1)
            defendButton.interactable = true;
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
    void TryDefend()
    {
        if ((Input.GetMouseButton(1) || defendButtonPressed) && Time.time - lastDefendTime > playerData.defendCooldown)
        {
            defendButtonPressed = false;
            state = PlayerState.Casting;
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
        if (state == PlayerState.Casting)
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
        if ((state == PlayerState.Idle || state == PlayerState.Moving) && Time.time - lastAttackTime > 1 / playerData.attackSpeed)
        {
            attackButtonPressed = true;
            attackButton.interactable = false;
        }
    }

    public void DefendButton()
    {
        if ((state == PlayerState.Idle || state == PlayerState.Moving) && Time.time - lastDefendTime > playerData.defendCooldown)
        {
            defendButtonPressed = true;
            defendButton.interactable = false;
        }
    }
}
