using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused;
    public static PauseManager instance;

    public delegate void PauseDelegate();
    public static PauseDelegate OnGamePause;
    public static PauseDelegate OnGameResume;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }
}
