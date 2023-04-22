using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] Vector2 _viewRange;
    [SerializeField] LayerMask _playerLayer;

    private GameObject target;

    Attack attack => null;
    Movement movement => null;
    Rect checkRect => Util.RectFromCenterSize(transform.position, _viewRange);


    public void Start()
    {
        // attack.Enable(this);
        // movement.Enable(this);
    }

    public void Update()
    {
        if (target == null)
        {
            var playerCollider = Physics2D.OverlapArea(checkRect.min, checkRect.max, _playerLayer);
            if (playerCollider != null)
            {
                target = playerCollider.gameObject;
            }
        }
    }

    //protected class EnemyAttack : HitscanAttack
    //{
    //    private Enemy _enemy;

    //    public override Entity entity => _enemy;
    //}
}
