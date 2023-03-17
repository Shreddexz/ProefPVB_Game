using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls;
    public int playerID;
    public bool isReady;
    public bool checkingReady;

    public Lane[] lanes;

    private void OnEnable()
    {
        PlayerManager.ReadyUp += OnReadyUp;
    }

    private void OnDisable()
    {
        PlayerManager.ReadyUp -= OnReadyUp;
    }
    void Awake()
    {
        isReady = false;
    }

    void Update()
    {
        if (checkingReady)
        {
            if (Input.GetKeyDown(controls.button1))
                isReady = true;
            return;
        }

        if (Input.GetKeyDown(controls.button1))
            lanes[1].NotePressed(playerID, SongVariables.playbackTime);

        if (Input.GetKeyDown(controls.button2))
            lanes[2].NotePressed(playerID, SongVariables.playbackTime);

        if (Input.GetKeyDown(controls.button3))
            lanes[3].NotePressed(playerID, SongVariables.playbackTime);

        if (Input.GetKeyDown(controls.joyL))
            lanes[0].NotePressed(playerID, SongVariables.playbackTime);

        if (Input.GetKeyDown(controls.joyR))
            lanes[1].NotePressed(playerID, SongVariables.playbackTime);
    }

    void OnReadyUp()
    {
        StartCoroutine(WaitForReady());
    }
    IEnumerator WaitForReady()
    {
        checkingReady = true;
        yield return new WaitUntil(() => isReady);
        checkingReady = false;
    }
}