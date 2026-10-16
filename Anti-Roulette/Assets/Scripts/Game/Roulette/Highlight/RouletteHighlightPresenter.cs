using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteHighlightPresenter : IRouletteHighlightActivator
{
    private readonly RouletteHighlightView _view;

    public RouletteHighlightPresenter(RouletteHighlightView view)
    {
        _view = view;
    }

    public void Activate()
    {
        _view.Show();
    }

    public void Deactivate()
    {
        _view.Hide();
    }
}

public interface IRouletteHighlightActivator
{
    public void Activate();
    public void Deactivate();
}
