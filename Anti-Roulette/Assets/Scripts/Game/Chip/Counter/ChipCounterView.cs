using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChipCounterView : View
{
    [SerializeField] private TextMeshProUGUI textCount;

    public void SetCount(int current, int all)
    {
        textCount.text = $"{current}/{all}";
    }
}
