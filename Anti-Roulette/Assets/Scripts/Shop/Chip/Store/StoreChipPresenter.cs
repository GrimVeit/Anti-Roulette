using System;
using System.Collections.Generic;

public class StoreChipPresenter : IChipInfoProvider, IChipListener, IChipProvider
{
    private readonly StoreChipModel _model;

    public StoreChipPresenter(StoreChipModel model)
    {
        _model = model;
    }

    public void Initialize()
    {
        _model.Initialize();
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    #region Provider

    public void OpenChip(int index)
    {
        _model.OpenChip(index);
    }

    public void SelectChip(int index)
    {
        _model.SelectChip(index);
    }

    #endregion

    #region Info

    public Chip GetChip(int index)
    {
        return _model.GetChip(index);
    }

    public IReadOnlyList<Chip> GetChips()
    {
        return _model.GetChips();
    }

    public Chip GetCurrentChip()
    {
        return _model.GetCurrentChip();
    }

    public int CurrentChipIndex => _model.GetCurrentChipIndex();

    public bool IsChipOpened(int index)
    {
        return _model.IsChipOpened(index);
    }

    public bool IsChipSelected(int index)
    {
        return _model.IsChipSelected(index);
    }

    #endregion

    #region Listener

    public event Action<Chip> OnOpenChip
    {
        add => _model.OnOpenChip += value;
        remove => _model.OnOpenChip -= value;
    }

    public event Action<Chip> OnSelectChip
    {
        add => _model.OnSelectChip += value;
        remove => _model.OnSelectChip -= value;
    }

    #endregion
}

public interface IChipProvider
{
    void OpenChip(int index);
    void SelectChip(int index);
}

public interface IChipInfoProvider
{
    Chip GetChip(int index);

    IReadOnlyList<Chip> GetChips();

    Chip GetCurrentChip();

    int CurrentChipIndex { get; }

    bool IsChipOpened(int index);

    bool IsChipSelected(int index);
}

public interface IChipListener
{
    event Action<Chip> OnOpenChip;
    event Action<Chip> OnSelectChip;
}