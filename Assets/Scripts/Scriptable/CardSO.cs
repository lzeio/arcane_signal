using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/CardScriptableObject", order = 1)]
public class CardSO : ScriptableObject
{
    public int Id;
    public Sprite cardSprite;


    /// Assigns a unique, sequential ID to each CardSO asset in the Unity Editor by finding the current max ID.
    /// Runs only in the Editor and preserves IDs for consistent use at runtime.

#if UNITY_EDITOR
    private void OnEnable()
    {
        if (Id != 0) return;

        AssignID();
    }

    private void OnValidate()
    {
        if (Id != 0) return;

        AssignID();
    }

    private void AssignID()
    {
        string[] guids = AssetDatabase.FindAssets("t:CardSO");
        int maxId = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            CardSO card = AssetDatabase.LoadAssetAtPath<CardSO>(path);

            if (card != null && card != this)
            {
                maxId = Mathf.Max(maxId, card.Id);
            }
        }

        Id = maxId + 1;
        EditorUtility.SetDirty(this);
    }
#endif
}