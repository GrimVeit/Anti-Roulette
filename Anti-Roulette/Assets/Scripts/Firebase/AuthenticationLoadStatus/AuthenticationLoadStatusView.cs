using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AuthenticationLoadStatusView : View
{
    [SerializeField] private Transform transformShow;
    [SerializeField] private Transform transformLoad;
    [SerializeField] private float durationShowHide;

    private Sequence seq;

    public void Load()
    {
        seq?.Kill();

        seq = DOTween.Sequence();

        seq.Append(transformShow.DOScale(0, durationShowHide));
        seq.Join(transformLoad.DOScale(1, durationShowHide));
    }

    public void Show()
    {
        seq?.Kill();

        seq = DOTween.Sequence();

        seq.Append(transformShow.DOScale(1, durationShowHide));
        seq.Join(transformLoad.DOScale(0, durationShowHide));
    }
}
