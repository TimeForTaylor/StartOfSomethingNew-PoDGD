using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public Vector2 lastCheckPointPos;

    private int keysFound;
    private int keysCollected;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad((instance));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddKey()
    {
        // add key

        keysFound++;
        Debug.Log("Keys found: " +keysFound);
    }
    
    public bool UseKey()
    {
        if (keysFound > 0)
        {
            keysFound--;
            return true;
        }

        return false;
    }
    
}
