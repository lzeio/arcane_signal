using UnityEngine;
using UnityEngine.UI;

public class GridLayoutController : MonoBehaviour
{
    [SerializeField] private LayoutConfigSO layoutConfig;
    [SerializeField] private GridLayoutGroup layoutGroup;

    public void Setup()
    {
        if (layoutConfig.rows <= 0 || layoutConfig.columns <= 0)
        {
            Debug.LogError("Invalid layout config");
            return;
        }

        layoutGroup.constraint =
            layoutConfig.rows > layoutConfig.columns
            ? GridLayoutGroup.Constraint.FixedRowCount
            : GridLayoutGroup.Constraint.FixedColumnCount;

        layoutGroup.constraintCount =
            layoutConfig.rows > layoutConfig.columns
            ? layoutConfig.rows
            : layoutConfig.columns;

        UpdateCellSize();
    }

    private void UpdateCellSize()
    {
        int rows = layoutConfig.rows;
        int columns = layoutConfig.columns;

        float baseSize = 100f;
        int reference = 5;

        float scale = Mathf.Min((float)reference / columns, (float)reference / rows);
        float finalSize = baseSize * scale;

        layoutGroup.cellSize = new Vector2(finalSize, finalSize);
    }

    public Transform GetParent() => layoutGroup.transform;

    public int GetPairCount() => (layoutConfig.rows * layoutConfig.columns) / 2;
}