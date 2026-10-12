using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChipPseudo : MonoBehaviour
{
    [SerializeField] private Image imageChip;

    private Tween tweenMove;

    public void SetData(Sprite spriteChip)
    {
        imageChip.sprite = spriteChip;
    }

    public void MoveTo(Transform transformPoint, float duration, Action OnComplete = null)
    {
        tweenMove?.Kill();

        tweenMove = transform.DOLocalMove(transformPoint.localPosition, duration)
            .OnComplete(() => OnComplete?.Invoke());
    }

    public void KillTween()
    {
        tweenMove?.Kill();
        tweenMove = null;
    }
}
