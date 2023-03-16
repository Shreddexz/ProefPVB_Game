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

    public void ToggleHowTo()
    {
        howToPanel.gameObject.SetActive(!howToPanel.gameObject.activeInHierarchy);
        if (!howToPanel.gameObject.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.Find("PlayButton"));
        }
    }

    public void ShowLevelSelect()
    {
        levelSelectionPanel.gameObject.SetActive(true);
        playerPanel.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("GH"));
    }

    public void HideLevelSelect()
    {
        levelSelectionPanel.gameObject.SetActive(false);
        playerPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("SP"));
    }

    public void ShowPlayerPanel()
    {
        playerPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("SP"));
        if (levelSelectionPanel.gameObject.activeInHierarchy)
        {
            HideLevelSelect();
        }
    }

    public void HidePlayerPanel()
    {
        playerPanel.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("PlayButton"));
    }

    public void LoadGameScene(string sceneName) => SceneManager.LoadScene(sceneName);

    public void ShowConfirmPanel()
    {
        confirmPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Yes"));
    }

    public void HideConfirmPanel()
    {
        confirmPanel.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("PlayButton"));
    }

    public void QuitGame()
    {
        Application.Quit();

#if(UNITY_EDITOR)
        EditorApplication.ExitPlaymode();
#endif
    }
}
