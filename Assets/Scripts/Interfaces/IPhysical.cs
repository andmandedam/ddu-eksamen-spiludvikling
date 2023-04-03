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
	public Rigidbody2D rigidbody {get;}
}



