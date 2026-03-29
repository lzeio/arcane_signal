using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    public static Action OnGameInit;
    public static Action<Card> OnCardClicked;

    [SerializeField] private AssetPackSO assetPack;
    [SerializeField] private Card cardPrefab;
    public Sprite backSprite;

    private Card firstSelected;
    private bool isBusy;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        OnGameInit += InitGameCards;
        OnCardClicked += HandleCardClick;
    }

    void OnDestroy()
    {
        OnGameInit -= InitGameCards;
        OnCardClicked -= HandleCardClick;
    }

    private void InitGameCards()
    {
        int pairCount = GameManager.Instance.GetTotalCards();

        if (assetPack.cardSOList.Count < pairCount)
        {
            Debug.LogError("Not enough card data!");
            return;
        }

        List<CardSO> deck = CreateShuffledDeck(pairCount);

        foreach (var cardSO in deck)
        {
            Card card = Instantiate(cardPrefab, GameManager.Instance.GetLayoutGroupTransform());
            card.SetCardData(cardSO.cardSprite, cardSO.id);
        }
    }

    private List<CardSO> CreateShuffledDeck(int pairCount)
    {
        List<CardSO> deck = new();

        for (int i = 0; i < pairCount; i++)
        {
            var cardData = assetPack.cardSOList[i];
            deck.Add(cardData);
            deck.Add(cardData);
        }

        Shuffle(deck);
        return deck;
    }

    private void Shuffle(List<CardSO> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    private void HandleCardClick(Card clicked)
    {
        if (isBusy || clicked == firstSelected) return;

        if (firstSelected == null)
        {
            firstSelected = clicked;
            return;
        }

        StartCoroutine(CheckMatch(clicked));
    }

    private IEnumerator CheckMatch(Card second)
    {
        isBusy = true;

        yield return new WaitForSeconds(0.5f);

        if (second.id == firstSelected.id)
        {
            second.Matched();
            firstSelected.Matched();
        }
        else
        {
            second.Unflip();
            firstSelected.Unflip();
        }

        firstSelected = null;
        isBusy = false;
    }
}