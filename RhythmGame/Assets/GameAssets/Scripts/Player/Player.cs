using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls;
    public int playerID;

    public Lane[] lanes;

    void Update()
    {
        if (Input.GetKeyDown(controls.button1))
            lanes[0].NotePressed(playerID, SongVariables.playbackTime);

        if (Input.GetKeyDown(controls.button2))
            lanes[1].NotePressed(playerID, SongVariables.playbackTime);

        if (Input.GetKeyDown(controls.button3))
            lanes[2].NotePressed(playerID, SongVariables.playbackTime);
    }
}