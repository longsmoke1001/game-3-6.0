using UnityEngine;

public class StackingArrow : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            if (source.TryGetComponent<Ranger>(out Ranger r))
                r.Attack(character);
            else
                character.TakeDamage(damage, source);
            if (projPierce)
                return;
            else if (projReturn)
                Return();
            else
                Destroy(gameObject);
        }
    }
}
