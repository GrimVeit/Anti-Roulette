using UnityEngine;

[CreateAssetMenu(fileName = "WheelData", menuName = "Game/Wheel Data")]
public sealed class WheelDataSO : ScriptableObject
{
    public int Index => index;
    public string Name => nameTable;
    public Sprite SpriteMain => spriteMain;
    public Sprite SpriteShop => spriteShop;
    public Sprite SpriteRoulette => spriteRoulette;
    public Sprite SpriteCross => spriteCross;
    public int Price => price;

    [SerializeField] private int index;
    [SerializeField] private string nameTable;
    [SerializeField] private Sprite spriteMain;
    [SerializeField] private Sprite spriteRoulette;
    [SerializeField] private Sprite spriteCross;
    [SerializeField] private Sprite spriteShop;
    [SerializeField] private int price;
}
