using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GridLayoutController grid;

    private int turnCount = 0;
    private int matchCount = 0;

    private int comboCount = 0;

    public static Action<int> OnTurnChanged;
    public static Action<int> OnMatchChanged;
    public static Action OnComboChanged;

    public static Action OnGameOver;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    void Start()
    {
        grid.Setup();
    }

    void OnEnable()
    {
        CardManager.OnCardClicked += IncrementTurnCount;
        CardManager.OnCardsMatched += IncrementMatchCount;
        CardManager.OnGameLoaded += LoadGame;
        OnMatchChanged += GameOver;

    }
    void OnDisable()
    {
        CardManager.OnCardClicked -= IncrementTurnCount;
        CardManager.OnCardsMatched -= IncrementMatchCount;
        CardManager.OnGameLoaded -= LoadGame;
        OnMatchChanged -= GameOver;
    }
    public int GetTurnCount() => turnCount;
    public int GetMatchCount() => matchCount;
    public int GetComboCount() => comboCount;

    public void ResetCombo()
    {
        comboCount = 0;
        OnComboChanged?.Invoke();
    }
    public Transform GetLayoutGroupTransform()
    {
        return grid.GetParent();
    }

    public int GetTotalCards()
    {
        return grid.GetPairCount();
    }

    private void IncrementTurnCount(Card _)
    {
        turnCount++;
        OnTurnChanged?.Invoke(turnCount);
    }

    private void IncrementMatchCount()
    {
        matchCount++;
        comboCount++;
        OnMatchChanged?.Invoke(matchCount);
    }


    private void LoadGame()
    {
        GameData data = SaveLoadManager.Instance.LoadGame();
        turnCount = data.turnCount;
        matchCount = data.matchCount;
        OnTurnChanged?.Invoke(turnCount);
        OnMatchChanged?.Invoke(matchCount);
    }


    private bool IsGameOver()
    {
        int totalPairs = grid.GetPairCount();
        return matchCount >= totalPairs;
    }

    private void GameOver(int _)
    {

        if (!IsGameOver())
            return;
        Debug.Log("Game Over! Total Turns: " + turnCount);
        // Optional: Show Game Over UI, reset game, etc.
        ResetGame();
        OnGameOver?.Invoke();
    }

    private void ResetGame()
    {
        turnCount = 0;
        matchCount = 0;
        comboCount = 0;
        OnTurnChanged?.Invoke(turnCount);
        OnMatchChanged?.Invoke(matchCount);
        OnComboChanged?.Invoke();
        SaveLoadManager.Instance.DeleteSave();
    }

}

