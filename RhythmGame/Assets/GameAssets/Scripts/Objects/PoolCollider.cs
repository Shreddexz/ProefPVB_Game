using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCollider : MonoBehaviour
{
    public LayerMask noteLayer;
    NotePooler pooler;
    ScoreManager scoreManager;

    void Awake()
    {
        pooler = transform.root.GetComponent<NotePooler>();
        scoreManager = transform.root.GetComponent<ScoreManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("p1"))
        {
            NoteEnemy enemy = other.gameObject.GetComponent<NoteEnemy>();
            if (!enemy.destroyed)
            {
                pooler.PoolObject(enemy);
                scoreManager.ReduceMultiplier(0);
            }
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("p2"))
        {
            NoteEnemy enemy = other.gameObject.GetComponent<NoteEnemy>();
            if (!enemy.destroyed)
            {
                pooler.PoolObject(enemy);
                scoreManager.ReduceMultiplier(1);
            }
        }
    }
}