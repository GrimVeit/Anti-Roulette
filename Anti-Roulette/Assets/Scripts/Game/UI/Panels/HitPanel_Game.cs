using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class HitPanel_Game : Panel
{
    [SerializeField] private SkeletonGraphic _skeletonGraphic;

    protected override void OnStartShow()
    {
        base.OnStartShow();

        _skeletonGraphic.AnimationState.SetAnimation(0, "win", false);
    }
}
