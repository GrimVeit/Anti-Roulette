using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderUser : LeaderboardUser
{
    public override void SetData(PlayerData data)
    {
        textNickname.text = data.Nickname;
        textMoney.text = $"{data.Money} points!";
    }
}
