using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveRotatePanel : Panel
{
    [Header("Move")]
    [SerializeField] private Vector3 moveFrom;
    [SerializeField] private Vector3 moveTo;

    [Header("Rotate")]
    [SerializeField] private Vector3 rotateFrom;
    [SerializeField] private Vector3 rotateTo;

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

        sequence.OnComplete(CompleteHide);
    }

    public override void Dispose()
    {
        sequence?.Kill();
        sequence = null;

        base.Dispose();
    }
}
