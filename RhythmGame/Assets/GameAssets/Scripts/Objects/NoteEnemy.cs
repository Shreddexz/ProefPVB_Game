using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoteEnemy : MonoBehaviour
{
    public double timeInstantiated;
    public double arriveTime;
    public float moveSpeed;
    public bool destroyed;
    [NonSerialized] public bool canMove;
    NoteManager manager;
    [NonSerialized] SpawnEffect destructionEffect;

    void Awake()
    {
        canMove = true;
        manager = GameObject.Find("GameManager").GetComponent<NoteManager>();
        destructionEffect = transform.GetChild(0).GetComponent<SpawnEffect>();
    }

    void Start()
    {
        NotePlaced();
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

    public void NotePlaced()
    {
        timeInstantiated = AudioManager.instance.playbackTime;
        arriveTime = timeInstantiated + SongVariables.twoBarsDuration;
        moveSpeed = SongVariables.bpm / 60 * manager.noteSpeedDistMultiplier;
    }

    public void Destroy()
    {
        destroyed = true;
        destructionEffect.playEffect = true;
    }
}