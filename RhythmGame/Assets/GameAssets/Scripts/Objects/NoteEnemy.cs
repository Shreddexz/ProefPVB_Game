using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEnemy : MonoBehaviour
{
    double timeInstantiated;
    double timeSinceSpawn;
    public float arriveTime;

    void Start()
    {
        timeInstantiated = Convert.ToDouble(AudioManager.instance.playbackTime);
    }

    void Update()
    {
        timeSinceSpawn = Convert.ToDouble(AudioManager.instance.playbackTime) - timeInstantiated;
        float time = (float) (timeSinceSpawn / (AudioManager.instance.noteTime * 2f));

        if (time > 1)
            Destroy(gameObject);
        else
        {
            transform.position = Vector3.Lerp(Vector3.forward * AudioManager.instance.noteOffset,
                                              Vector3.forward * -AudioManager.instance.noteOffset, time);
        }
    }
}