using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle
{
    public delegate void AttackEventHandler(Cell cell, bool result);
    public event AttackEventHandler OnAttack;

    public bool Attack(Cell cell)
    {

        if (!cell.IsEmpty)
        {
            cell.IsEmpty = true;
            OnAttack?.Invoke(cell, true);
            return true;
        }
        else
        {
            OnAttack?.Invoke(cell, false);
            return false;
        }
    }

    public bool CheckSurroundingCells(Grid grid, Cell attackedCell)
    {
        int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
        int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

        foreach (var cell in grid.OccupiedCells)
        {
            if (!cell.IsEmpty)
            {
                int distanceX = Mathf.Abs(cell.PosX - attackedCell.PosX);
                int distanceY = Mathf.Abs(cell.PosY - attackedCell.PosY);

                if (distanceX <= 1 && distanceY <= 1)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
