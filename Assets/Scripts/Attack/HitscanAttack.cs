using System;
using UnityEngine;

[Serializable]
public class HitscanAttack : Attack
{
    [Header("HitscanAttack")]
    [SerializeField] Vector2 _hitSize;
    [SerializeField] LayerMask _attackLayer;
    [SerializeField] int _attackDamage;
    [SerializeField] float _attackKnockback;

    public Rect hitRect => Util.RectFromCenterSize(attackPoint, hitSize);
    public Vector2 hitSize => _hitSize;
    public LayerMask attackLayer => _attackLayer;
    public int attackDamage => _attackDamage;
    public float attackKnockback => _attackKnockback;

    public virtual Vector2 attackPoint => (Vector2)transform.position + actor.facing;

    public override void OnAttack()
    {
        base.OnAttack();
        Debug.LogFormat(
            "HitRect: {0}"
            , hitRect
        );
        Util.DrawRect(hitRect);

        Vector2 min = hitRect.min, max = hitRect.max;
        var colliders = Physics2D.OverlapAreaAll(min, max, attackLayer);

        Debug.Log(colliders);

        foreach (var collider in colliders)
        {
            Debug.Log(collider);
            if (collider.TryGetComponent(out Entity hit))
            {
                hit.Damage(entity, attackDamage);

                Vector2 pos = hit.transform.position;
                Vector2 dir = (hit.rigidbody.position - (Vector2)entity.transform.position);

                // var raycast = Physics2D.Raycast(attackPoint, dir, 1.5f * dir.magnitude, attackLayer);
                // 
                // if (raycast.rigidbody == hit.rigidbody)
                // {
                // pos = raycast.point;
                // }
                // else
                // {
                // pos = hit.transform.position;
                // }

                hit.Knockback(entity, dir.normalized * attackKnockback, pos);
            }
        }
    }
}
