using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSelectVisualModel
{
    private readonly IChipInfoProvider _chipInfoProvider;
    private readonly IChipListener _chipListener;
    private readonly IChipProvider _chipProvider;

    public ChipSelectVisualModel(IChipInfoProvider chipInfoProvider, IChipListener chipListener, IChipProvider chipProvider)
    {
        _chipInfoProvider = chipInfoProvider;
        _chipListener = chipListener;
        _chipProvider = chipProvider;
    }

    public void Initialize()
    {
        _chipListener.OnOpenChip += Open;
        _chipListener.OnSelectChip += Select;

        OnSetChips?.Invoke(_chipInfoProvider.GetChips());
        OnSelect?.Invoke(_chipInfoProvider.CurrentChipIndex);
    }

    public void Dispose()
    {
        _chipListener.OnOpenChip -= Open;
        _chipListener.OnSelectChip -= Select;
    }

    public void Select(int index)
    {
        if (_chipInfoProvider.CurrentChipIndex == index) return;

        if (!_chipInfoProvider.GetChip(index).IsOpened) return;

        OnDeselect?.Invoke(_chipInfoProvider.CurrentChipIndex);

        _chipProvider.SelectChip(index);
    }

    private void Open(Chip chip)
    {
        OnOpen?.Invoke(chip.Index);
    }

    private void Select(Chip chip)
    {
        OnSelect?.Invoke(chip.Index);
    }

    #region Output

    public event Action<IReadOnlyList<Chip>> OnSetChips;
    public event Action<int> OnOpen;
    public event Action<int> OnSelect;
    public event Action<int> OnDeselect;

    #endregion
}
