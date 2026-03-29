using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    // Events
    public static Action OnGameInit;
    public static Action OnGameLoaded;
    public static Action<Card> OnCardClicked;
    public static Action OnCardsMatched;

    // Inspector fields 
    [SerializeField] private AssetPackSO assetPack;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Sprite backSprite;

    private List<Card> selectionBuffer = new List<Card>();
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        OnGameInit += InitGameCards;
        OnGameLoaded += LoadGameCards;
        OnCardClicked += HandleCardClick;
    }

    void OnDisable()
    {
        OnGameInit -= InitGameCards;
        OnGameLoaded -= LoadGameCards;
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
            card.Init(cardSO.cardSprite, cardSO.id);
        }
    }

    private void LoadGameCards()
    {
        // Placeholder for loading saved game state
        // Would need to track flipped/matched states and reapply them here
        Debug.Log("LoadGameCards called - implement save/load logic here");
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
        if (selectionBuffer.Contains(clicked)) return;
        selectionBuffer.Add(clicked);

        if (selectionBuffer.Count >= 2)
        {
            Card first = selectionBuffer[0];
            Card second = selectionBuffer[1];

            selectionBuffer.RemoveRange(0, 2);

            StartCoroutine(CheckMatchRoutine(first, second));
        }
    }

    private IEnumerator CheckMatchRoutine(Card first, Card second)
    {
        // Optional: wait for flip animation to finish
        yield return new WaitUntil(() => !first.IsFlipping && !second.IsFlipping);

        if (first.id == second.id)
        {
            first.Matched();
            second.Matched();
            OnCardsMatched?.Invoke();
        }
        else
        {
            yield return new WaitForSeconds(0.3f); // small delay feels good

            first.Unflip();
            second.Unflip();
        }
    }
    public Sprite GetBackSprite() => backSprite;
}