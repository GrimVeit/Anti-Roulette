using UnityEngine;
using UnityEngine.UI;

public class WheelVisualView : View
{
    [SerializeField] private Image image_Main;
    [SerializeField] private Image image_Numbers;
    [SerializeField] private Image image_Cross;

    public void SetDesign(Wheel wheel)
    {
        image_Main.sprite = wheel.SpriteMain;
        image_Numbers.sprite = wheel.SpriteRoulette;
        image_Cross.sprite = wheel.SpriteCross;
    }
}
