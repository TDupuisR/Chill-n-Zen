using System.Collections.Generic;
using UnityEngine;
using GameManagerSpace;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] int _levelNumber;
    [SerializeField] int _levelBudget;
    [SerializeField] int _levelScore;
    [Space(10)]
    [SerializeField] List<GridSetup> gridInstructions;
    [SerializeField] DoorSetup doorInstruction;
    [Space(10)]
    [SerializeField] GMStatic.Request _primaryRequests;
    [SerializeField] GMStatic.Request _secondaryRequests;

    public enum GridGen { Create, Delete }
    public enum Rotation { SW, SE, NE, NW }

    // Grid //
    [System.Serializable]
    public struct GridSetup
    {
        public GridGen instruction;
        public Vector2Int StartPos;
        public Vector2Int GridSize;
    }
    [System.Serializable]
    public struct DoorSetup
    {
        public Vector2Int Position;
        public Rotation orientation;
    }

    private void Start()
    {
        GridMethod();
        DoorMethod();

        GameManager.requestManager.Initialisation(_primaryRequests, _secondaryRequests);
        GameManager.budgetManager.CurrentBudget = _levelBudget;
        GameManager.budgetManager.OnSetDefaultBudget?.Invoke();
        GameManager.levelManager.LevelNumber = _levelNumber;
        GameManager.levelManager.ScoreToReach = _levelScore;

        Destroy(gameObject);
    }

    private void GridMethod()
    {
        foreach (GridSetup gridSetup in gridInstructions)
        {
            if (gridSetup.instruction == GridGen.Create)
            {
                TileSystem.Instance.GenerateGrid(gridSetup.StartPos, gridSetup.GridSize);
            }
            else if (gridSetup.instruction == GridGen.Delete)
            {
                if (gridSetup.GridSize == Vector2Int.zero)
                    TileSystem.Instance.DeleteGrid();
                else
                    TileSystem.Instance.DeleteGrid(gridSetup.StartPos, gridSetup.GridSize);
            }
        }
    }
    private void DoorMethod()
    {
        int rotation = 0;
        if (doorInstruction.orientation == Rotation.SW) rotation = 0;
        else if (doorInstruction.orientation == Rotation.SE) rotation = 90;
        else if (doorInstruction.orientation == Rotation.NE) rotation = 180;
        else if (doorInstruction.orientation == Rotation.NW) rotation = 270;

        TileSystem.Instance.PlaceDoor(doorInstruction.Position);
        TileSystem.Instance.RotateDoor(rotation);
    }
}
