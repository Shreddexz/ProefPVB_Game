using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static bool multiplayer;

    void OnEnable()
    {
        DontDestroyOnLoad(this);
    }

    //void Awake()
    //{
    //    multiplayer = true;
    //}

    public void SetMultiplayer(bool isTrue)
    {
        multiplayer = isTrue;
    }
}