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
    }
    void OnDisable()
    {
        CardManager.OnCardClicked -= IncrementTurnCount;
        CardManager.OnCardsMatched -= IncrementMatchCount;
    }
    public int GetTurnCount() => turnCount;
    public int GetMatchCount() => matchCount;
    public int GetComboCount() => comboCount;
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
        comboCount = 0;
        OnTurnChanged?.Invoke(turnCount);
    }

    private void IncrementMatchCount()
    {
        matchCount++;
        comboCount++;
        OnMatchChanged?.Invoke(matchCount);
    }

}

