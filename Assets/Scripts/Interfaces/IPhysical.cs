using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Enemy : MonoBehaviour, IPhysical
//{
//    public Rigidbody2D GetRigidbody() => gameObject.GetComponent<Rigidbody2D>();
//}

public interface IPhysical 
{
    // impl
    public Rigidbody2D GetRigidbody();

    public void Push(Vector2 dir)
    {
        Rigidbody2D rb = GetRigidbody();
        rb.AddForce(dir);
    }
}



