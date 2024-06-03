using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GridManager gridManager1;
    public GridManager gridManager2;

    private void Start()
    {
        var player1OccupiedCells = GameManager.Instance.Player1OccupiedCells;
        gridManager1.FillOccupiedCells(player1OccupiedCells);

        var player2OccupiedCells = GameManager.Instance.Player2OccupiedCells;
        Debug.Log(player2OccupiedCells.Count);
        gridManager2.FillOccupiedCells(player2OccupiedCells);

        //foreach(var cell in player2OccupiedCells)
        //{
        //    Debug.Log($"{cell.PosX} : {cell.PosY} : {cell.IsEmpty}");
        //    gridManager2.grid.AddOccupiedCells(cell);
        //}
    }
}
