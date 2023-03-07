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

    NoteManager manager;

    void Awake()
    {
        canMove = true;
        manager = GameObject.Find("GameManager").GetComponent<NoteManager>();
    }

    void Start()
    {
        timeInstantiated = AudioManager.instance.playbackTime;
        moveSpeed = (AudioManager.bpm / 60) * manager.noteSpeedDistMultiplier;
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