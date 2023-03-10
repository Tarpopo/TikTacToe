using System;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class Grids : ManagerBase, IStart
{
    public Grid CurrentGrid => _grids[_currentGrid];

    public Grid[] AllGrids => _grids;
    [SerializeField] private Grid[] _grids;
    private int _currentGrid;

    private void EnableGrid(GridType gridType)
    {
        foreach (var grid in _grids) grid.SetActive(false);
        _currentGrid = (int)gridType;
        CurrentGrid.SetActive(true);
    }

    public void ClearAllGrids()
    {
        foreach (var grid in _grids) grid.ClearCells();
    }

    public void Enable3X3() => EnableGrid(GridType.ThreeXThree);
    public void Enable5X5() => EnableGrid(GridType.FiveXFive);

    public void OnStart()
    {
        _grids[0].SetCombination(3);
        _grids[1].SetCombination(5);
    }
}

[Serializable]
public class Grid
{
    public WinCombination WinCombination { get; private set; }
    public int GridSize { get; private set; }

    public Cell[] Cells => _cells;
    public Cell RandomFreeCell => _cells.Where(cell => cell.Sign == null).GetRandomElement();
    public bool CellsExist => _cells.Any(cell => cell.Sign == null);
    public Signs?[] Signs => _cells.Select(cell => cell.Sign).ToArray();
    [SerializeField] private GameObject _grid;
    [SerializeField] private Cell[] _cells;

    public void SetCombination(int size)
    {
        GridSize = size;
        WinCombination = new WinCombination(size);
    }

    public bool TryGetMiddleCell(out Cell middleCell)
    {
        var cell = _cells[_cells.Length / 2];
        middleCell = cell;
        return cell.Sign == null;
    }

    public void SetActive(bool active) => _grid.SetActive(active);

    public bool TryGetWinCellsCombination(Signs sign, out Cell[] cells)
    {
        cells = default;
        if (WinCombination.CheckWin(_cells, sign, out var winCell) == false) return false;
        cells = new Cell[WinCombination.GridSize];
        for (int i = 0; i < winCell.WinCombination.Length; i++) cells[i] = _cells[winCell.WinCombination[i]];
        return true;
    }

    public bool CheckWin(Signs sign) => WinCombination.CheckWin(_cells, sign);

    public bool CheckWin(Signs sign, Signs?[] cells) => WinCombination.CheckWin(sign, cells);

    public void ClearCells()
    {
        foreach (var cell in _cells) cell.ClearCell();
    }
}

public enum GridType
{
    ThreeXThree,
    FiveXFive,
}