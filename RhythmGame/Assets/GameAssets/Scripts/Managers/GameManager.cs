using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1, player2;
    public float splitscreenFOV = 80f;
    Camera p2cam;

    void Start()
    {
        if (PlayerManager.multiplayer)
        SplitScreen();
    }

    void SplitScreen()
    {
        player2.gameObject.SetActive(true);
        p2cam = player2.transform.GetChild(0).GetComponent<Camera>();
        p2cam.rect = new Rect(0.5f, 0f, 0.5f, 1f);
        p2cam.fieldOfView = splitscreenFOV;
        Camera.main.rect = new Rect(0f, 0f, 0.5f, 1f);
        Camera.main.fieldOfView = splitscreenFOV;
    }
}