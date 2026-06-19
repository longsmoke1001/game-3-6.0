using UnityEngine;
using UnityEngine.TextCore.Text;

public class StackingArrow : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Character>(out Character character) && !character.hitAttackId.Contains(projectileId) && source.TryGetComponent<Ranger>(out Ranger r))
            r.stacks++;
        base.OnTriggerEnter2D(collision);
    }
}
