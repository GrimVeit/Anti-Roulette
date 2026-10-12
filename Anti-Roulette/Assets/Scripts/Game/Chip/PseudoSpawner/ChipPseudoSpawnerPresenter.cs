using System;

public class ChipPseudoSpawnerPresenter : IChipPseudoListener, IChipPseudoProvider
{
    private readonly ChipPseudoSpawnerModel _model;
    private readonly ChipPseudoSpawnerView _view;

    public ChipPseudoSpawnerPresenter(ChipPseudoSpawnerModel model, ChipPseudoSpawnerView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _model.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    private void ActivateEvents()
    {
        _model.OnSpawnChip += _view.SpawnChip;
        _model.OnDelete += _view.Delete;
        _model.OnMoveStart += _view.MoveToStart;
        _model.OnReload += _view.Clear;
    }

    private void DeactivateEvents()
    {
        _model.OnSpawnChip -= _view.SpawnChip;
        _model.OnDelete -= _view.Delete;
        _model.OnMoveStart -= _view.MoveToStart;
        _model.OnReload -= _view.Clear;
    }

    #region Input

    public void MoveToStart() => _model.MoveToStart();
    public void Delete() => _model.Delete();
    public void Reload() => _model.Reload();

    #endregion

    #region Output

    public event Action<int> OnCurrentCount
    {
        add => _model.OnCurrentCount += value;
        remove => _model.OnCurrentCount -= value;
    }

    public event Action OnStartMove
    {
        add => _view.OnStartMove += value;
        remove => _view.OnStartMove -= value;
    }

    public event Action OnEndMove
    {
        add => _view.OnEndMove += value;
        remove => _view.OnEndMove -= value;
    }

    public event Action OnReload
    {
        add => _model.OnReload += value;
        remove => _model.OnReload -= value;
    }

    #endregion
}

public interface IChipPseudoListener
{
    public event Action<int> OnCurrentCount;

    public event Action OnStartMove;
    public event Action OnEndMove;
}

public interface IChipPseudoProvider
{
    public void Reload();
    public void MoveToStart();
    public void Delete();
}
