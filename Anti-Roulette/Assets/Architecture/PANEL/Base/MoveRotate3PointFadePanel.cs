using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MoveRotate3PointFadePanel : Panel
{
    [Header("Move")]
    [SerializeField] private Vector3 moveFrom;
    [SerializeField] private Vector3 moveOpen;
    [SerializeField] private Vector3 moveExit;

    [Header("Rotate")]
    [SerializeField] private Vector3 rotateFrom;
    [SerializeField] private Vector3 rotateOpen;
    [SerializeField] private Vector3 rotateExit;

    [Header("Fade")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Animation")]
    [SerializeField] private float duration = 0.25f;

    private Sequence sequence;

    protected override void OnStartShow()
    {
        sequence?.Kill();

        transform.localPosition = moveFrom;
        transform.localEulerAngles = rotateFrom;
        canvasGroup.alpha = 0f;

        sequence = DOTween.Sequence()
            .SetUpdate(true);

        sequence.Join(
            transform.DOLocalMove(moveOpen, duration)
        );

        sequence.Join(
            transform.DOLocalRotate(rotateOpen, duration)
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
            transform.DOLocalMove(moveExit, duration)
        );

        sequence.Join(
            transform.DOLocalRotate(rotateExit, duration)
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
