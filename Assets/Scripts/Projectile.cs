using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;
    public Character source;
    float startingTime;
    [SerializeField] protected float projectileTime = 3f;
    public bool projReturn;
    // Start is called before the first frame update
    void Start()
    {
        startingTime = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right.normalized * speed * Time.deltaTime);
        if (Time.time - startingTime > projectileTime)
            CheckReturn();

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(damage, source);
            CheckReturn();
        }
    }

    void CheckReturn()
    {
        if (projReturn)
        {
            transform.Rotate(0, 0, 180);
            startingTime= Time.time;
            projReturn = false;
        }
        else 
            Destroy(gameObject);
    }
}
