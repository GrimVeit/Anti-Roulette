using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class RotateFadePanel : Panel
{
    [Header("Rotation")]
    [SerializeField] private Vector3 from;
    [SerializeField] private Vector3 to;

    [Header("Fade")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Animation")]
    [SerializeField] private float duration = 0.25f;

    private Sequence sequence;

    protected override void OnStartShow()
    {
        sequence?.Kill();

        sequence = DOTween.Sequence()
            .SetUpdate(true);

        sequence.Join(
            transform.DOLocalRotate(to, duration)
        );

        sequence.Join(
            canvasGroup.DOFade(1f, duration)
        );

        sequence.OnComplete(CompleteShow);
    }

    protected override void OnStartHide()
    {
        sequence?.Kill();

        sequence = DOTween.Sequence()
            .SetUpdate(true);

        sequence.Join(
            transform.DOLocalRotate(from, duration)
        );

        sequence.Join(
            canvasGroup.DOFade(0f, duration)
        );

        sequence.OnComplete(CompleteHide);
    }

    public override void Dispose()
    {
        sequence?.Kill();
        sequence = null;

        base.Dispose();
    }
}
