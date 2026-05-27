using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    public float damage = 10f;
    public Character source;
    float startingTime;
    float projectileTime = 3f;
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
            Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character))
            {
                character.TakeDamage(damage, source);
                Destroy(gameObject);
            }
    }
}
