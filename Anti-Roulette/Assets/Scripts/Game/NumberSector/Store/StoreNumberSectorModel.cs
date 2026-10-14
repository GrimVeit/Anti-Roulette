using System;
using System.Collections.Generic;

public class StoreNumberSectorModel
{
    public NumberColor CurrentColorNumber => _currentColorNumber;

    private NumberColor _currentColorNumber;
    private readonly List<NumberColor> _sectors;
    private readonly Random _random = new();

    public StoreNumberSectorModel()
    {
        _sectors = new List<NumberColor>
        {
            new(0, RouletteColor.Green),
            new(1, RouletteColor.Red),
            new(2, RouletteColor.Black),
            new(3, RouletteColor.Red),
            new(4, RouletteColor.Black),
            new(5, RouletteColor.Red),
            new(6, RouletteColor.Black),
            new(7, RouletteColor.Red),
            new(8, RouletteColor.Black),
            new(9, RouletteColor.Red),
            new(10, RouletteColor.Black),
            new(11, RouletteColor.Black),
            new(12, RouletteColor.Red),
            new(13, RouletteColor.Black),
            new(14, RouletteColor.Red),
            new(15, RouletteColor.Black),
            new(16, RouletteColor.Red),
            new(17, RouletteColor.Black),
            new(18, RouletteColor.Red),
            new(19, RouletteColor.Red),
            new(20, RouletteColor.Black),
            new(21, RouletteColor.Red),
            new(22, RouletteColor.Black),
            new(23, RouletteColor.Red),
            new(24, RouletteColor.Black),
            new(25, RouletteColor.Red),
            new(26, RouletteColor.Black),
            new(27, RouletteColor.Red),
            new(28, RouletteColor.Black),
            new(29, RouletteColor.Black),
            new(30, RouletteColor.Red),
            new(31, RouletteColor.Black),
            new(32, RouletteColor.Red),
            new(33, RouletteColor.Black),
            new(34, RouletteColor.Red),
            new(35, RouletteColor.Black),
            new(36, RouletteColor.Red)
        };
    }

    public void ChooseRandom()
    {
        if (_sectors == null || _sectors.Count == 0)
            return;

        int index = _random.Next(_sectors.Count);
        _currentColorNumber = _sectors[index];

        OnChoose?.Invoke(_currentColorNumber);
    }

    #region Output

    public event Action<NumberColor> OnChoose;

    #endregion
}

public enum RouletteColor
{
    Red,
    Green,
    Black
}

public class NumberColor
{
    public int Number { get; set; }
    public RouletteColor Color { get; set; }

    public NumberColor(int number, RouletteColor color)
    {
        Number = number;
        Color = color;
    }
}
