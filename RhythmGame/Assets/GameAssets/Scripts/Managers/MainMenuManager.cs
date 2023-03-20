using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public RectTransform howToPanel;
    public RectTransform levelSelectionPanel;
    public RectTransform confirmPanel;
    public RectTransform playerPanel;

    /// <summary>
    /// Toggles the 'How to play' panel.
    /// When disabling the panel, the first button in the home menu is targeted.
    /// </summary>
    public void ToggleHowTo()
    {
        howToPanel.gameObject.SetActive(!howToPanel.gameObject.activeInHierarchy);
        if (!howToPanel.gameObject.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.Find("PlayButton"));
        }
    }

    /// <summary>
    /// Enables the level selection panel, and targets the song that's on that panel as active button.
    /// </summary>
    public void ShowLevelSelect()
    {
        levelSelectionPanel.gameObject.SetActive(true);
        playerPanel.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("GH"));
    }

    /// <summary>
    /// Hides the level selection panel and targets the 'singleplayer' button as active button.
    /// </summary>
    public void HideLevelSelect()
    {
        levelSelectionPanel.gameObject.SetActive(false);
        playerPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("SP"));
    }

    /// <summary>
    /// shows the player selection panel, and targets the 'singleplayer' button as active button.
    /// </summary>
    public void ShowPlayerPanel()
    {
        playerPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("SP"));
        if (levelSelectionPanel.gameObject.activeInHierarchy)
        {
            HideLevelSelect();
        }
    }

    /// <summary>
    /// hides the player selection panel,and targets the 'Play' button as active button.
    /// </summary>
    public void HidePlayerPanel()
    {
        playerPanel.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("PlayButton"));
    }

    /// <summary>
    /// Loads a scene by name, which is used for loading a scene when pressing a UI button.
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadGameScene(string sceneName) => SceneManager.LoadScene(sceneName);

    /// <summary>
    /// Shows a confirmation panel when trying to exit the game, and targets the 'yes' button.
    /// </summary>
    public void ShowConfirmPanel()
    {
        confirmPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Yes"));
    }

    /// <summary>
    /// Hides the confirmation panel , and targets the 'play' button.
    /// </summary>
    public void HideConfirmPanel()
    {
        confirmPanel.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("PlayButton"));
    }

    /// <summary>
    /// Closes the game.
    /// When in the editor, playmode will be exited.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();

#if(UNITY_EDITOR)
        EditorApplication.ExitPlaymode();
#endif
    }
}
