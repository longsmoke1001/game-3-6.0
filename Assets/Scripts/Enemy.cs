using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Character
{
    [SerializeField] protected HealthBar healthBarPrefab;
    [SerializeField] protected Transform healthBarParent;
    [SerializeField] protected float movSpeed=2f;
    protected Player player;
    protected HealthBar healthBar;
    bool isDead;
    protected Animator anim;
    protected Vector2 spawnPos;
    protected EnemyState state;
    [SerializeField] protected GameManager gameManager;
    public enum EnemyState
    {
        Patrolling,
        Attacking,
        Chasing,
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {   
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
