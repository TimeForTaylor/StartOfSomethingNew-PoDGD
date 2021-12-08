using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatBall : MonoBehaviour
{
    public GameObject Toast_SpriteSheet;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit Detected");
        GameObject e = Instantiate(Toast_SpriteSheet) as GameObject;
        e.transform.position = transform.position;
        Destroy(other.gameObject);
        this.gameObject.SetActive(false);
    }
}
