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
    public ParticleSystem explosion, flames;

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

    /// <summary>
    /// moves the note at a constant speed
    /// </summary>
    void MoveNote()
    {
        if (canMove)
            transform.position -= Vector3.forward * (moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Sets the movementspeed to the song's BPM to sync everything with the music, sets the time the note was instantiated,
    /// and sets the time the note arrives at the indicator line.
    /// Called when the note is (re)instantiated;
    /// </summary>
    public void NotePlaced()
    {
        timeInstantiated = AudioManager.instance.playbackTime;
        arriveTime = timeInstantiated + SongVariables.twoBarsDuration;
        moveSpeed = SongVariables.bpm / 60 * manager.noteSpeedDistMultiplier;
    }


    /// <summary>
    /// Plays the destruction effect of the spaceships, and destroys this object after 2 seconds.
    /// </summary>
    public void Destroy()
    {
        destroyed = true;
        destructionEffect.playEffect = true;
        Destroy(gameObject, 2f);
        explosion.Play();
        flames.Play();
    }
}