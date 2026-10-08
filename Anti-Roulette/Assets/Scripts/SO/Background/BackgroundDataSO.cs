using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundData", menuName = "Game/Background Data")]
public sealed class BackgroundDataSO : ScriptableObject
{
    public int Index => index;
    public string Name => nameTable;
    public Sprite Sprite => sprite;
    public Sprite SpriteShop => spriteShop;
    public int Price => price;

    [SerializeField] private int index;
    [SerializeField] private string nameTable;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Sprite spriteShop;
    [SerializeField] private int price;
}

