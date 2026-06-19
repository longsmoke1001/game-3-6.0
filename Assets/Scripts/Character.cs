using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [field: SerializeField] public float currHealth { get; protected set; } = 100f;
    [field: SerializeField] public float maxHealth { get; protected set; } = 100f;
    public HashSet<int> hitAttackId= new HashSet<int>();
    protected int projectileSpreadAngle = 15;
    protected GameManager gameManager;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        currHealth = maxHealth;
        gameManager = FindAnyObjectByType<GameManager>();

    }
    public virtual void TakeDamage(float amount,Character source)
    {
        Debug.Log("TakeDamage: " + amount);
        currHealth = Mathf.Max(0f, currHealth - amount);
    }

    public void Shoot(int typeId, Character target,float damage,float projectileSpeed, int projectileCount, int projectileSpread, bool projectileReturn, bool projectilePierce)
    {
        Vector2 toEnemy = target.transform.position - transform.position;
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject arrow = ObjectPooler.SharedInstance.GetPooledObject(typeId);
            arrow.SetActive(true);
            Projectile a = arrow.GetComponent<Projectile>();
            a.projectileId = gameManager.projectileId;
            a.transform.position = transform.position;
            a.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(toEnemy.y, toEnemy.x) * Mathf.Rad2Deg + 15 * (projectileCount - 1) - 30 * i);
            if (projectileReturn)
                a.projReturn = true;
            if (projectilePierce)
                a.projPierce = true;
            a.speed = projectileSpeed;
            a.source = this;
            a.damage = damage;
        }
        gameManager.projectileId++;
    }

    public void Shoot(int typeId, Character target,float damage)
    {
        Shoot(typeId, target, damage, 10, 1, 15, false, false);
    }
}
