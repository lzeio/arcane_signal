using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    public static CardManager Instance { get; private set; }
    public AssetPackSO assetPack;

    public static Action OnGameInit;
    public static Action<Card> OnCardsClicked;
    public Card cardPrefab;

    private Card lastClickedCard = null;

    void Awake()
    {
        Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
        }

        OnGameInit += InitGameCards;
        OnCardsClicked += TryMatchingCards;
        // InitGameCards();
    }

    void OnDisable()
    {
        OnGameInit -= InitGameCards;
    }

    public void InitGameCards()
    {
        int totalCards = GameManager.Instance.GetTotalCards();

        if (assetPack.cardSOList.Count < totalCards)
        {
            Debug.LogError("Not enough card data to fill the grid!");
            return;
        }

        List<CardSO> shuffledList = new List<CardSO>();

        // Create pairs
        for (int i = 0; i < totalCards; i++)
        {
            shuffledList.Add(assetPack.cardSOList[i]);
            shuffledList.Add(assetPack.cardSOList[i]);
        }

        // Shuffle
        Shuffle(shuffledList);

        // Instantiate
        foreach (var cardSO in shuffledList)
        {
            Card newCard = Instantiate(cardPrefab, GameManager.Instance.GetLayoutGroupTransform());
            newCard.SetCardData(cardSO.cardSprite, cardSO.id);
        }
    }

    void Shuffle(List<CardSO> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    void TryMatchingCards(Card card1)
    {
        if (lastClickedCard == card1) return;
        if (lastClickedCard == null)
        {
            lastClickedCard = card1; return;
        }
        if (card1.id == lastClickedCard.id)
        {
            card1.Matched();
            lastClickedCard.Matched();
            lastClickedCard = null;
            Debug.Log("Cards Matched!");
        }
        else
        {
            card1.Unflip();
            lastClickedCard.Unflip();
            lastClickedCard = null;
        }
    }
}
