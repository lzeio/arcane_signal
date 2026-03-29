using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI matchCounterText;
    [SerializeField] private TextMeshProUGUI turnCounterText;


    void Awake()
    {
        GameManager.OnTurnChanged += UpdateTurnCounter;
        GameManager.OnMatchChanged += UpdateMatchCounter;
    }

    void OnDisable()
    {
        GameManager.OnTurnChanged -= UpdateTurnCounter;
        GameManager.OnMatchChanged -= UpdateMatchCounter;
    }

    private void UpdateTurnCounter(int turns)
    {
        Debug.Log("Turn Count: " + turns);
        turnCounterText.text = $"Turns\n<color=red>{turns}</color>";
    }

    private void UpdateMatchCounter(int matches)
    {
        matchCounterText.text = $"Matches\n<color=red>{matches}</color>";
    }
}
