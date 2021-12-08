using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toaster : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.tag == "Toast")
       {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
       }
    }
    
}
