using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LayoutConfigSO layoutConfig;

    public GridLayoutGroup layoutGroup;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        SetupLayoutGroup();
    }
    public void SetupLayoutGroup()
    {
        if (layoutGroup != null)
        {

            if (layoutConfig.rows <= 0 || layoutConfig.columns <= 0 || layoutConfig.rows > 4 || layoutConfig.columns > 5)
            {
                Debug.LogError("Invalid layout configuration! Rows and columns must be greater than 0 and less than 4 and 5 respectively.");
                return;
            }
            layoutGroup.constraint = layoutConfig.rows > layoutConfig.columns ? GridLayoutGroup.Constraint.FixedRowCount : GridLayoutGroup.Constraint.FixedColumnCount;
            layoutGroup.constraintCount = layoutConfig.rows > layoutConfig.columns ? layoutConfig.rows : layoutConfig.columns;
        }
    }

    public Transform GetLayoutGroupTransform()
    {
        return layoutGroup.transform;
    }

    public int GetTotalCards()
    {
        return (layoutConfig.rows * layoutConfig.columns) / 2;
    }
}
