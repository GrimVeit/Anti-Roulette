using TMPro;
using UnityEngine;

public class LeaderboardUser : MonoBehaviour
{
    [SerializeField] protected private TextMeshProUGUI textNickname;
    [SerializeField] protected private TextMeshProUGUI textMoney;
    [SerializeField] private UIEffect uIEffect;

    public virtual void SetData(PlayerData data)
    {
        textNickname.text = data.Nickname;
        textMoney.text = data.Money.ToString();
    }

    public void ResetClear()
    {
        uIEffect.ResetEffect();

        transform.localScale = Vector3.zero;
    }

    public void Show()
    {
        uIEffect.PlayShow();
    }

    public void Hide()
    {
        uIEffect.PlayHide();
    }
}
