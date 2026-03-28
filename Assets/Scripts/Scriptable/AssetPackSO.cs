using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AssetPack", menuName = "AssetPackScriptableObject", order = 1)]
public class AssetPackSO : ScriptableObject
{
    public List<CardSO> cardSOList;
}
