using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Cell
{
    private int _posX;
    private int _posY;
    private bool _isEmpty = true;
    public bool _isClickable = true;

    public int PosX
    {
        get { return _posX; }
    }

    public int PosY
    {
        get { return _posY; }
    }

    public bool IsEmpty
    {
        get { return _isEmpty; }
        set { _isEmpty = value; }
    }

    public Cell(int x, int y)
    {
        _posX = x;
        _posY = y;
    }
}