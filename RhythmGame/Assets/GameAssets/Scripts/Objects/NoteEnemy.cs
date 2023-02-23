using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEnemy : MonoBehaviour
{
    public double timeInstantiated;
    // double timeSinceSpawn;
    public double arriveTime;
    public float moveSpeed;
    [NonSerialized]
    public bool canMove;

    void Awake()
    {
        canMove = true;
    }

    void Start()
    {
        timeInstantiated = AudioManager.instance.playbackTime;
        moveSpeed = 60 * 4 / AudioManager.bpm;
    }

    void Update()
    {
        MoveNote();
    }

    void MoveNote()
    {
        if (canMove)
            transform.position -= Vector3.forward * moveSpeed * Time.deltaTime;
    }
}