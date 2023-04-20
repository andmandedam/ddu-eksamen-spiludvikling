using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public override LayerMask platformLayer => throw new System.NotImplementedException();

    protected class EnemyAttack : HitscanAttack
    {
        private Enemy _enemy;

        public override Entity entity => _enemy;
    }
}
