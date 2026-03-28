using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    public static CardManager Instance { get; private set; }


    public List<CardSO> cardSOList;

    public static Action OnGameInit;

    public Card cardPrefab;

    void Awake()
    {
        Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
        }

        OnGameInit += InitGameCards;
        InitGameCards();
    }

    void OnDisable()
    {
        OnGameInit -= InitGameCards;
    }

    public void InitGameCards()
    {
        foreach (CardSO cardSO in cardSOList)
        {
            Debug.Log($"Card ID: {cardSO.Id}, Sprite: {cardSO.cardSprite.name}");
            Card cardGO = Instantiate(cardPrefab, FindAnyObjectByType<Canvas>().transform);
            cardGO.SetCardData(cardSO.cardSprite, cardSO.Id);
        }
    }
}
