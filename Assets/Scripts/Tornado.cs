using UnityEngine;

public class Tornado : MonoBehaviour
{
    public float damage;
    public Character source;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.TryGetComponent(out Character character);
        if (character != null && character != source)
        {
            character.TakeDamage(damage, source);
        }
    }
}
