using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitscanAttack : Attack
{
    [SerializeField] Vector2 _hitSize;
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _attackKnockback;
    [SerializeField] private LayerMask _attackLayer;
    [SerializeField] private float _windupTime;
    [SerializeField] private float _attackTime;
    [SerializeField] private float _cooldownTime;


    public Rect hitRect
    {
        get
        {
            return Util.RectFromCenterSize(attackPoint, _hitSize);
        }
    }
    public int attackDamage => _attackDamage;
    public float attackKnockback => _attackKnockback;
    public LayerMask attackLayer => _attackLayer;
    public override float windupTime => _windupTime;
    public override float attackTime => _attackTime;
    public override float cooldownTime => _cooldownTime;
    public virtual Vector2 attackPoint => entity.transform.position;



    public static void DrawRect(Rect r)
    {
        Vector2[] verticies =
        {
            new Vector2(r.xMin, r.yMin),
            new Vector2(r.xMin, r.yMax),
            new Vector2(r.xMax, r.yMax),
            new Vector2(r.xMax, r.yMin)
        };
        for (int i = 0; i < verticies.Length; i++)
        {
            Debug.DrawLine(verticies[i % 4], verticies[(i + 1) % 4], Color.black, 1000, false);
        }
    }

    public override void OnAttack()
    {
        base.OnAttack();

        DrawRect(hitRect);
        Debug.Log(hitRect);

        Vector2 min = hitRect.min, max = hitRect.max;
        var colliders = Physics2D.OverlapAreaAll(min, max, attackLayer);
        Debug.Log(colliders);

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
