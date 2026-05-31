using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [field: SerializeField] public float currHealth { get; protected set; } = 100f;
    [field: SerializeField] public float maxHealth { get; protected set; } = 100f;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        //maxHealth = GetComponent<Enemy>().maxHealth;
        currHealth = maxHealth;
    }

    public virtual void TakeDamage(float amount,Character source)
    {
        Debug.Log("TakeDamage: " + amount);
        currHealth = Mathf.Max(0f, currHealth - amount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
