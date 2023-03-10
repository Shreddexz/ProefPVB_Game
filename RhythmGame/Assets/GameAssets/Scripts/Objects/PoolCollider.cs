using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCollider : MonoBehaviour
{
    public LayerMask noteLayer;
    NotePooler pooler;
    void Awake()
    {
        pooler = transform.root.GetComponent<NotePooler>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Note"))
        {
            NoteEnemy enemy = other.gameObject.GetComponent<NoteEnemy>();
            pooler.PoolObject(enemy);
        }
    }
}
