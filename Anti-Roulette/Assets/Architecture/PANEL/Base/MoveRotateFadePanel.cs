using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MoveRotateFadePanel : Panel
{
    [Header("Move")]
    [SerializeField] private Vector3 moveFrom;
    [SerializeField] private Vector3 moveTo;

    [Header("Rotate")]
    [SerializeField] private Vector3 rotateFrom;
    [SerializeField] private Vector3 rotateTo;

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
            transform.DOLocalMove(moveTo, duration)
        );

        sequence.Join(
            transform.DOLocalRotate(rotateTo, duration)
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
            transform.DOLocalMove(moveFrom, duration)
        );

        sequence.Join(
            transform.DOLocalRotate(rotateFrom, duration)
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
