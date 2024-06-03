using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


public class Grid
{
    private List<Cell> _occupiedCells;

    public List<Cell> OccupiedCells
    {
        get { return _occupiedCells; }
    }

    public Grid()
    {
        _occupiedCells = new List<Cell>();
    }



    public void AddOccupiedCells(Cell cell)
    {
        _occupiedCells.Add(cell);
    }

    public bool CanPlaceShip(Cell cell)
    {
        if (!cell.IsEmpty)
        {
            return false;
        }

        foreach (var occupiedCell in _occupiedCells)
        {
            int distanceX = Math.Abs(occupiedCell.PosX - cell.PosX);
            int distanceY = Math.Abs(occupiedCell.PosY - cell.PosY);

            if (distanceX <= 1 && distanceY <= 1)
            {
                return false;
            }
        }

        return true;
    }

    public void RemoveOccupiedCell(Cell cell)
    {
        _occupiedCells.Remove(cell);
    }

    public bool RotateShip(Ships ship)
    {
        var occupiedCells = ship.GetOccupiedCells();
        var minCell = occupiedCells.OrderBy(c => c.PosX).ThenBy(c => c.PosY).First();
        List<Cell> newCells = new List<Cell>();

        if (ship.IsHorizontal)
        {
            for (int i = 0; i < ship.Health; i++)
            {
                int newX = minCell.PosX;
                int newY = minCell.PosY - i;

                if (newY < 0 || _occupiedCells.Any(c => c.PosX == newX && c.PosY == newY && !c.IsEmpty))
                {
                    return RotateShipDown(ship);
                }

                newCells.Add(new Cell(newX, newY));
            }
        }
        else
        {
            for (int i = 0; i < ship.Health; i++)
            {
                int newX = minCell.PosX + i;
                int newY = minCell.PosY;

                if (newX < 0 || _occupiedCells.Any(c => c.PosX == newX && c.PosY == newY && !c.IsEmpty))
                {
                    return false;
                }

                newCells.Add(new Cell(newX, newY));
            }
        }

        foreach (var cell in occupiedCells)
        {
            cell.IsEmpty = true;
        }
        foreach (var cell in newCells)
        {
            cell.IsEmpty = false;
            AddOccupiedCells(cell);
        }
        ship.SetOccupiedCells(newCells.ToArray());
        ship.IsHorizontal = !ship.IsHorizontal;

        return true;
    }

    private bool RotateShipDown(Ships ship)
    {
        var occupiedCells = ship.GetOccupiedCells();
        var minCell = occupiedCells.OrderBy(c => c.PosX).ThenBy(c => c.PosY).First();
        List<Cell> newCells = new List<Cell>();

        for (int i = 0; i < ship.Health; i++)
        {
            int newX = minCell.PosX;
            int newY = minCell.PosY + i;

            if (newY < 0 || _occupiedCells.Any(c => c.PosX == newX && c.PosY == newY && !c.IsEmpty))
            {
                return false; 
            }

            newCells.Add(new Cell(newX, newY));
        }

        foreach (var cell in occupiedCells)
        {
            cell.IsEmpty = true;
        }
        foreach (var cell in newCells)
        {
            cell.IsEmpty = false;
            AddOccupiedCells(cell);
        }
        ship.SetOccupiedCells(newCells.ToArray());
        ship.IsHorizontal = !ship.IsHorizontal;

        return true;
    }
}
