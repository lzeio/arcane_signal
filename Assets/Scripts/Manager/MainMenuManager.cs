using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] Button PlayButton;

    //public List<LayoutConfigSO> layoutConfigs;
    void OnEnable()
    {
        PlayButton.onClick.AddListener(OnPlayButtonClicked);
    }

    void OnDisable()
    {
        PlayButton.onClick.RemoveListener(OnPlayButtonClicked);
    }

    public void OnPlayButtonClicked()
    {
        mainMenuPanel.SetActive(false);
        CardManager.OnGameInit?.Invoke();
    }


}
