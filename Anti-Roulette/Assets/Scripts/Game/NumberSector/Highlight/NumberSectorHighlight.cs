using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class NumberSectorHighlight : MonoBehaviour
{
    private const string HighlightAnimation = "target";

    public int Number => number;

    [SerializeField] private int number;
    [SerializeField] private Transform transformHighlight;
    [SerializeField] private SkeletonGraphic skeletonGraphic;

    private Tween tweenScale;

    public void Show(float durationShow)
    {
        tweenScale?.Kill();

        // Гарантируем, что скелет активен и анимация запущена с нуля
        skeletonGraphic.enabled = true;
        skeletonGraphic.AnimationState.SetAnimation(0, HighlightAnimation, true);

        tweenScale = transformHighlight.DOScale(1f, durationShow);
    }

    public void Hide(float durationHide)
    {
        tweenScale?.Kill();

        if (durationHide <= 0f)
        {
            CompleteHide();
            return;
        }

        tweenScale = transformHighlight
            .DOScale(0f, durationHide)
            .OnComplete(CompleteHide);
    }

    private void CompleteHide()
    {
        // Завершаем/очищаем анимацию, чтобы loop не крутился вхолостую
        skeletonGraphic.AnimationState.ClearTracks();
        skeletonGraphic.Skeleton.SetToSetupPose();

        // Отключаем рендер/апдейт скелета — самый дешёвый вариант
        skeletonGraphic.enabled = false;
    }
}
