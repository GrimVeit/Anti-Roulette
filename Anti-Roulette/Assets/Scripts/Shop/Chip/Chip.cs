using UnityEngine;

public class Chip
{
    public int Index { get; }
    public string Name { get; }
    public Sprite Sprite { get; }
    public Sprite SpriteShop { get; }
    public int Price { get; }
    public bool IsOpened { get; private set; }

    public Chip(int index, string name, Sprite spriteShop, Sprite sprite, int price, bool isOpened)
    {
        Index = index;
        Name = name;
        SpriteShop = spriteShop;
        Sprite = sprite;
        Price = price;
        IsOpened = isOpened;
    }

    public void Open()
    {
        IsOpened = true;
    }

    public void Close()
    {
        IsOpened = false;
    }
}
