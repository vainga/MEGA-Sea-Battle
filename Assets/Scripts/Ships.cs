public class Ships
{
    private bool _isHorizontal = true;

    public bool IsHorizontal
    {
        get { return _isHorizontal; }
        set { _isHorizontal = value; }
    }

    private int _health;
    public int Health
    {
        get { return _health; }
    }

    private Cell[] _occupiedCells;

    public Ships(int health)
    {
        _health = health;
        _occupiedCells = new Cell[_health];
    }

    public void SetOccupiedCells(Cell[] cells)
    {
        _occupiedCells = cells;
    }

    public Cell[] GetOccupiedCells()
    {
        return _occupiedCells;
    }

    public void ClearOccupiedCells()
    {
        if (_occupiedCells != null)
        {
            foreach (var cell in _occupiedCells)
            {
                if (cell != null)
                {
                    cell.IsEmpty = true;
                }
            }
        }

        for (int i = 0; i < _occupiedCells.Length; i++)
        {
            _occupiedCells[i] = null;
        }
    }

}
