using UnityEngine;

public class Wheel
{
    public int Index { get; }
    public string Name { get; }
    public Sprite SpriteShop { get; }
    public Sprite SpriteMain { get; }
    public Sprite SpriteRoulette {  get; }
    public Sprite SpriteCross {  get; }
    public int Price { get; }

    public bool IsOpened { get; private set; }

    public Wheel(
        int index,
        string name,
        Sprite spriteMain,
        Sprite spriteShop,
        Sprite spriteRoulette,
        Sprite spriteCross,
        int price,
        bool isOpened)
    {
        Index = index;
        Name = name;
        SpriteMain = spriteMain;
        SpriteShop = spriteShop;
        SpriteRoulette = spriteRoulette;
        SpriteCross = spriteCross;
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
