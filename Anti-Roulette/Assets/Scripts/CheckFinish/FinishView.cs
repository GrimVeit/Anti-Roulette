using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class FinishView : View
{
    [SerializeField] private GameObject objectWin;
    [SerializeField] private GameObject objectLose;
    [SerializeField] SkeletonGraphic skeletonGraphic;

    public void Win()
    {
        objectWin.SetActive(true);
        objectLose.SetActive(false);
        skeletonGraphic.AnimationState.SetAnimation(0, "Win", true);
    }

    public void Lose()
    {
        objectWin.SetActive(false);
        objectLose.SetActive(true);
        skeletonGraphic.AnimationState.SetAnimation(0, "fail", true);
    }
}
