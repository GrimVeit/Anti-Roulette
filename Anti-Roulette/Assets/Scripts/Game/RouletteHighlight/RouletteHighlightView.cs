using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class RouletteHighlightView : View
{
    [SerializeField] private SkeletonGraphic skeletonGraphic;
    [SerializeField] private Transform transformHighlight;
    [SerializeField] private float duration;

    private Tween tweenScale;

    public void Show()
    {
        tweenScale?.Kill();

        // Гарантируем, что скелет активен и анимация запущена с нуля
        skeletonGraphic.enabled = true;
        skeletonGraphic.AnimationState.SetAnimation(0, "wheel", true);

        tweenScale = transformHighlight.DOScale(1f, duration);
    }

    public void Hide()
    {
        tweenScale?.Kill();

        tweenScale = transformHighlight.DOScale(0f, duration).OnComplete(CompleteHide);
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
