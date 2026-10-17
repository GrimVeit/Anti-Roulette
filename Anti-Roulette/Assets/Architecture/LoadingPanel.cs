using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MovePanel
{
    [SerializeField] private List<Panel> panels = new();

    public override void Initialize()
    {
        base.Initialize();

        panels.ForEach(p => p.Initialize());
    }

    public override void Dispose()
    {
        base.Dispose();

        panels.ForEach(p => p.Dispose());
    }

    protected override void OnStartShow()
    {
        base.OnStartShow();

        panels.ForEach(p => p.Show());
    }

    protected override void OnStartHide()
    {
        base.OnStartHide();

        panels.ForEach(p => p.Hide());
    }
}
