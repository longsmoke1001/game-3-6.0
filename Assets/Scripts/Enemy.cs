using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Character
{
    [SerializeField] protected HealthBar healthBarPrefab;
    [SerializeField] protected Transform healthBarParent;
    [SerializeField] protected float movSpeed = 2f;
    protected Player player;
    protected HealthBar healthBar;
    bool isDead;
    protected Animator anim;
    protected Vector2 spawnPos;

    [SerializeField] protected GameManager gameManager;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = FindAnyObjectByType<Player>();
        spawnPos = transform.position;
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();
        gameManager.AddEnemy(this);
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
        if (healthBarPrefab != null)
        {
            Debug.Log("healthBarPrefab is not null");
            Transform parent = healthBarParent;
            if (parent == null)
            {
                Canvas canvas = FindAnyObjectByType<Canvas>();
                if (canvas != null)
                    parent = canvas.transform;
            }
            healthBar = Instantiate(healthBarPrefab, parent);
            healthBar.Init(this);
        }
    }

    public void Init()
    {
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();
        maxHealth *= 1 + ((int)GlobalGameManager.Instance.currentLevel - 1) / gameManager.Levels.Count;
        currHealth = maxHealth;
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
