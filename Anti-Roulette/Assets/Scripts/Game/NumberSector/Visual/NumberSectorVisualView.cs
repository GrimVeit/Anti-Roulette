using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberSectorVisualView : View
{
    [SerializeField] private TextMeshProUGUI textNumber;
    [SerializeField] private TextMeshProUGUI textColor;

    public void SetNumberColor(NumberColor numberColor)
    {
        textNumber.text = numberColor.Number.ToString();
        textColor.text = numberColor.Color.ToString();
    }
}
