using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitscanAttack : Attack
{
    public virtual Vector2 attackPoint => entity.transform.position;
    public abstract Rect hitRect { get; }
    public abstract int attackDamage { get; }
    public abstract float attackKnockback { get; }
    public abstract LayerMask attackLayer { get; }

    public override void OnAttack()
    {
        Vector2 min = hitRect.min, max = hitRect.max;
        var colliders = Physics2D.OverlapAreaAll(min, max, attackLayer);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out Entity hit))
            {
                hit.Damage(entity, attackDamage);

                Vector2 pos;
                Vector2 dir = (hit.rigidbody.position - attackPoint);

                var raycast = Physics2D.Raycast(hit.transform.position, dir, 1.5f * dir.magnitude, attackLayer);

                if (raycast.rigidbody == hit.rigidbody)
                {
                    pos = raycast.point;
                }
                else
                {
                    pos = hit.transform.position;
                }

                hit.Knockback(entity, dir.normalized * attackKnockback, pos);
            }
        }
    }
}
