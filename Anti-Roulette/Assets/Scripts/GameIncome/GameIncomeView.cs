using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameIncomeView : View
{
    [SerializeField] private TextMeshProUGUI textMeshIncome;
    [SerializeField] private string textIncome = "You won! You earned {0} coins";

    public void SetMoney(int money)
    {
        textMeshIncome.text = string.Format(textIncome, money);
    }
}
