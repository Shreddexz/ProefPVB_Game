using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public Player player1;
    public Player player2;
    public static bool multiplayer;
    public static bool playersReady;

    public delegate void ReadyUpDelegate();

    public static ReadyUpDelegate ReadyUp;
    public static ReadyUpDelegate AllReady;
    public static bool player1Ready, player2Ready;
    void OnEnable()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnGameLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnGameLoaded;
    }

    void Awake()
    {
        playersReady = false;
    }

    private void Update()
    {
        if (!player1)
            return;

        player1Ready = player1.isReady;
        if (player2)
            player2Ready = player2.isReady;

        if (multiplayer && !player2)
            return;

        if (playersReady)
            return;

        if (!multiplayer)
            playersReady = player1.isReady ? true : false;
        else
            playersReady = player1.isReady && player2.isReady ? true : false;
    }

    public void SetMultiplayer(bool isTrue)
    {
        multiplayer = isTrue;
    }

    public void OnGameLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Gameplay")
        {
            StartCoroutine(LoadWait());
        }
    }

    IEnumerator WaitForReady()
    {

        while (!playersReady)
            yield return null;
        AllReady?.Invoke();
    }

    IEnumerator LoadWait()
    {
        yield return new WaitForSeconds(0.2f);
        player1 = GameObject.Find("player 1").GetComponent<Player>();
        if (multiplayer)
            player2 = GameObject.Find("player 2").GetComponent<Player>();

        StartCoroutine(WaitForReady());
        ReadyUp?.Invoke();
    }

}