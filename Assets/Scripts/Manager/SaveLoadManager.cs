using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }
    private GameData _gameData;
    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
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
        _gameData = new GameData
        {
            rows = GridLayoutController.Instance.GetRows(),
            cols = GridLayoutController.Instance.GetCols(),
            cards = new List<CardData>()
        };
        CardManager.OnCardClicked += SaveGame;
    }
    public void SaveGame(Card _)
    {
        Debug.Log("Saved Data Triggered");
        List<Card> cards = CardManager.Instance.GetAllCards();
        _gameData.turnCount = GameManager.Instance.GetTurnCount();
        _gameData.matchCount = GameManager.Instance.GetMatchCount();

        _gameData.cards.Clear();

        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];

            _gameData.cards.Add(new CardData
            {
                id = card.id,
                isMatched = card.isMatched,
            });
        }

        string json = JsonUtility.ToJson(_gameData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Saved Data: " + json);
        Debug.Log("Game Saved → " + SavePath);
    }

    public GameData LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("No save file found");
            return null;
        }

        try
        {
            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        catch
        {
            Debug.LogError("Save file corrupted");
            return null;
        }
    }

    public bool HasSave()
    {
        return File.Exists(SavePath);
    }

    public void DeleteSave()
    {
        if (HasSave())
        {
            File.Delete(SavePath);
            Debug.Log("Save Deleted");
        }
    }


}

[System.Serializable]
public class GameData
{
    public int rows;
    public int cols;
    public List<CardData> cards;
    public int turnCount;
    public int matchCount;
}

[System.Serializable]
public class CardData
{
    public int id;
    public bool isMatched;
}