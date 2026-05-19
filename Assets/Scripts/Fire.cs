using UnityEngine;

public class Fire : MonoBehaviour
{
    public float damage = 5f;
    public Character source;
    bool contacted = false;
    float startingTime;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Fire hit: " + collision.gameObject.name);
        if (collision.TryGetComponent<Player>(out Player player) && !contacted&&source!=null)
        {
            player.TakeDamage(damage, source);
            contacted = true;
        }

    }
}
