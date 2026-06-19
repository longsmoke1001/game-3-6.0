using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Character
{
    [SerializeField] protected HealthBar healthBarPrefab;
    protected Transform healthBarParent;
    [SerializeField] protected float movSpeed = 2f;
    [SerializeField] protected float attackPower = 5f;
    protected Player player;
    protected HealthBar healthBar;
    bool isDead;
    protected Animator anim;
    protected Vector2 spawnPos;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = FindAnyObjectByType<Player>();
        spawnPos = transform.position;
        gameManager.AddEnemy(this);
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
        if (healthBarPrefab != null)
        {
            healthBar = Instantiate(healthBarPrefab, GameObject.Find("InstantiateCanvas").transform);
            healthBar.Init(this);
        }
    }

    public void Init()
    {
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();
        maxHealth *= GlobalGameManager.Instance.difficultyMultiplier/2*Mathf.Pow(3,(((int)GlobalGameManager.Instance.currentLevel - 1) / gameManager.Levels.Count));
        currHealth = maxHealth;
        attackPower *= GlobalGameManager.Instance.difficultyMultiplier / 2 * Mathf.Pow(1.5f, (((int)GlobalGameManager.Instance.currentLevel - 1) / gameManager.Levels.Count));
        if (GlobalGameManager.Instance.currentLevel != 15&& GlobalGameManager.Instance.currentLevel != 10)
            for (int i = 0; i < (int)(GlobalGameManager.Instance.currentLevel - 1) / gameManager.Levels.Count; i++)
            {
                Enemy enemy_ = Instantiate(gameObject, transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0), Quaternion.identity).GetComponent<Enemy>();
            }

    }

    protected virtual void Update()
    {
        if (!isDead && currHealth <= 0f)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
    void OnDestroy()
    {
        if (gameManager != null)
            gameManager.RemoveEnemy(this);

        if (healthBar != null)
            Destroy(healthBar.gameObject);
    }

}
