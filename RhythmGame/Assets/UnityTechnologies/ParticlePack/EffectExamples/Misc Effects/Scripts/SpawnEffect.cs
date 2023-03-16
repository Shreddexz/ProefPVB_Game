using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    public float spawnEffectTime = 2;
    public float pause = 1;
    public AnimationCurve fadeIn;
    
    float timer = 0;
    Renderer _renderer;
    [NonSerialized]public bool playEffect;
    int shaderProperty;

    void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            playEffect = true;

        if (playEffect)
            Effect();
    }

    void Effect()
    {
        if (timer < spawnEffectTime + pause)
        {
            timer += Time.deltaTime;
        }

        _renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));
        Destroy(gameObject, spawnEffectTime);
    }
}