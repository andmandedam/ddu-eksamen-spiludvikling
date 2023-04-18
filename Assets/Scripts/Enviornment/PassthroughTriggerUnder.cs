using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughTriggerUnder : PassthroughTrigger
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        base.AllowPassthroughFor(collider);
    }
}
