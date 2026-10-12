using System;

public class ChipPseudoSpawnerModel
{
    private readonly IChipInfoProvider _chipInfoProvider;

    private readonly int _allCount;
    private int _currentCount;

    public ChipPseudoSpawnerModel(IChipInfoProvider chipInfoProvider, int spawnCount)
    {
        _chipInfoProvider = chipInfoProvider;
        _allCount = spawnCount;
    }

    public void Initialize()
    {
        _currentCount = _allCount;
        OnCurrentCount?.Invoke(_currentCount);

        for (int i = 0; i < _allCount; i++)
        {
            OnSpawnChip?.Invoke(_chipInfoProvider.GetCurrentChip());
        }
    }

    public void MoveToStart()
    {
        if (_currentCount <= 0) return;

        _currentCount -= 1;
        OnMoveStart?.Invoke();

        OnCurrentCount?.Invoke(_currentCount);
    }

    public void Delete()
    {
        OnDelete?.Invoke();
    }

    public void Reload()
    {
        _currentCount = _allCount;

        OnCurrentCount?.Invoke(_currentCount);

        OnReload?.Invoke();

        for (int i = 0; i < _allCount; i++)
        {
            OnSpawnChip?.Invoke(_chipInfoProvider.GetCurrentChip());
        }
    }

    #region Output

    public event Action<Chip> OnSpawnChip;
    public event Action OnMoveStart;
    public event Action OnDelete;
    public event Action OnReload;

    public event Action<int> OnCurrentCount;

    #endregion
}