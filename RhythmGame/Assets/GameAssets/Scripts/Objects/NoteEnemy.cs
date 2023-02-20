using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEnemy : MonoBehaviour
{
    public double timeInstantiated;
    double timeSinceSpawn;
    public float arriveTime;
    public float moveSpeed;

    void Start()
    {
        timeInstantiated = AudioManager.instance.pbt;
        moveSpeed = AudioManager.bpm / 60f;
    }

    void Update()
    {
        // timeSinceSpawn = AudioManager.instance.pbt - timeInstantiated;
        // float time = (float) (timeSinceSpawn / (AudioManager.instance.noteTime * 2f));
        //
        // if (time > 1)
        //     Destroy(gameObject);
        // else
        // {
        //     transform.position = Vector3.Lerp(Vector3.forward * AudioManager.instance.noteOffset,
        //                                       Vector3.forward * -AudioManager.instance.noteOffset, time);
        // }
        transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
    }
}