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

    public delegate void ReadyUpDelegate();//A delegate that is used for the ready-up mechanics.

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
    /// <summary>
    /// This method is used to set the multiplayer boolean by clicking a button.
    /// </summary>
    /// <param name="isTrue"></param>
    public void SetMultiplayer(bool isTrue)
    {
        multiplayer = isTrue;
    }

    /// <summary>
    /// When a scene is loaded, the name is checked for "Gameplay", which is the scene in which the game is played.
    /// If it returns true, other methods can be called.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    public void OnGameLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Gameplay")
        {
            StartCoroutine(LoadWait());
        }
    }

    /// <summary>
    /// This coroutine waits for the player(s) to ready up, and fires the event that starts the game when it returns true.
    /// </summary>
    IEnumerator WaitForReady()
    {
        while (!playersReady)
            yield return null;
        AllReady?.Invoke();
    }

    /// <summary>
    /// Returns the player components in the scene, and then proceeds to fire off the event that starts the ready up process.
    /// To prevent a null-reference exception when looking got the player components, there is a small wait at the start of the coroutine.
    /// </summary>
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