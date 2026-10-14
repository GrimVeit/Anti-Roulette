using System;
using Cysharp.Threading.Tasks;
using System.Threading;

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

    public void Dispose() => DeactivateEvents();

    private void ActivateEvents()
    {
        _model.OnSpawnChip += _view.SpawnChip;
        _model.OnDelete += _view.Delete;
        _model.OnReload += _view.Clear;
        // OnMoveStart Ќ≈ пробрасываем во View Ч View дЄргаетс€ из MoveToStartAsync
    }

    private void DeactivateEvents()
    {
        _model.OnSpawnChip -= _view.SpawnChip;
        _model.OnDelete -= _view.Delete;
        _model.OnReload -= _view.Clear;
    }

    #region Input

    public async UniTask MoveToStartAsync(CancellationToken token)
    {
        if (!_model.MoveToStart()) return;
        await _view.MoveToStartAsync(token);
    }

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

    #endregion
}

public interface IChipPseudoListener
{
    event Action<int> OnCurrentCount;

    event Action OnStartMove;
    event Action OnEndMove;
}

public interface IChipPseudoProvider
{
    UniTask MoveToStartAsync(CancellationToken token);
    void Reload();
    void Delete();
}
