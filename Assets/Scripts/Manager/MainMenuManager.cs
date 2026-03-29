using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//this class can be used to manage main menu interactions, for now it just has the play button to start the game, but we can expand it later with options, settings, etc.
// this class is sakura right now since ui manager can handle this.
public class MainMenuManager : MonoBehaviour
{

    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] Button PlayButton;
    [SerializeField] Button ResumeButton;
    void OnEnable()
    {
        PlayButton.onClick.AddListener(OnPlayButtonClicked);
        ResumeButton.onClick.AddListener(OnResumeButtonClicked);

    }

    void OnDisable()
    {
        PlayButton.onClick.RemoveListener(OnPlayButtonClicked);
        ResumeButton.onClick.RemoveListener(OnResumeButtonClicked);
    }

    public void OnPlayButtonClicked()
    {
        mainMenuPanel.SetActive(false);
        CardManager.OnGameInit?.Invoke();
    }

    public void OnResumeButtonClicked()
    {
        mainMenuPanel.SetActive(false);
        CardManager.OnGameLoaded?.Invoke();
    }


}
