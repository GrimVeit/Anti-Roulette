using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Rotate3PointFadePanel : Panel
{
    [Header("Rotations")]
    [SerializeField] private Vector3 from;
    [SerializeField] private Vector3 open;
    [SerializeField] private Vector3 exit;

    [Header("Fade")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Animation")]
    [SerializeField] private float duration = 0.25f;

    private Sequence sequence;

    protected override void OnStartShow()
    {
        sequence?.Kill();

        transform.localEulerAngles = from;
        canvasGroup.alpha = 0f;

        sequence = DOTween.Sequence()
            .SetUpdate(true);

        sequence.Join(
            transform.DOLocalRotate(open, duration)
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
            transform.DOLocalRotate(exit, duration)
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
