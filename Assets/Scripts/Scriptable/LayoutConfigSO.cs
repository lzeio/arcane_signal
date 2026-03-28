using UnityEngine;

[CreateAssetMenu(fileName = "LayoutConfig", menuName = "LayoutConfigScriptableObject", order = 1)]
public class LayoutConfigSO : ScriptableObject
{

    [Header("Grid Settings")]
    public int rows;
    public int columns;
}
