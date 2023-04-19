using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public override float staticDrag => throw new System.NotImplementedException();
    public override float dynamicDrag => throw new System.NotImplementedException();

    public override LayerMask platformLayer => throw new System.NotImplementedException();

    public override void Damage(Entity source, int damage)
    {
        Debug.LogFormat("{0} took {1} damage from {2}", this, damage, source);
        base.Damage(source, damage);
    }
}
