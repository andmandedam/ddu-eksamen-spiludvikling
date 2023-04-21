using System;
using UnityEngine;

public abstract class HitscanAttack : Attack
{
    [SerializeField] Vector2 _hitSize;
    [SerializeField] int _attackDamage;
    [SerializeField] float _attackKnockback;
    [SerializeField] LayerMask _attackLayer;
    [SerializeField] float _windupTime;
    [SerializeField] float _attackTime;
    [SerializeField] float _cooldownTime;

    public Rect hitRect => Util.RectFromCenterSize(attackPoint, _hitSize);
    public int attackDamage => _attackDamage;
    public float attackKnockback => _attackKnockback;
    public LayerMask attackLayer => _attackLayer;
    public override float windupTime => _windupTime;
    public override float attackTime => _attackTime;
    public override float cooldownTime => _cooldownTime;

    public abstract Vector2 attackPoint { get; }

    public override void AttackEntry()
    {
        base.AttackEntry();
        // Debug.Log(hitRect);
        // Util.DrawRect(hitRect);

        Vector2 min = hitRect.min, max = hitRect.max;
        var colliders = Physics2D.OverlapAreaAll(min, max, attackLayer);
        // Debug.Log(colliders);

        foreach (var collider in colliders)
        {
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
