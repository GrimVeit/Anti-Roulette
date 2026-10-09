using System;
using System.Collections.Generic;

public class ChipShopVisualModel
{
    private readonly IChipInfoProvider _chipInfoProvider;
    private readonly IChipListener _chipListener;
    private readonly IChipProvider _chipProvider;
    private readonly IMoneyProvider _moneyProvider;

    public ChipShopVisualModel(IChipInfoProvider chipInfoProvider, IChipListener chipListener, IChipProvider chipProvider, IMoneyProvider moneyProvider)
    {
        _chipInfoProvider = chipInfoProvider;
        _chipListener = chipListener;
        _chipProvider = chipProvider;
        _moneyProvider = moneyProvider;
    }

    public void Initialize()
    {
        _chipListener.OnOpenChip += OpenChip;

        OnSetChips?.Invoke(_chipInfoProvider.GetChips());
    }

    public void Dispose()
    {
        _chipListener.OnOpenChip -= OpenChip;
    }

    private void OpenChip(Chip chip)
    {
        OnOpenChip?.Invoke(chip.Index);
    }

    public void Buy(int index)
    {
        if (_chipInfoProvider.IsChipOpened(index)) return;

        var chip = _chipInfoProvider.GetChip(index);

        if (!_moneyProvider.CanAfford(chip.Price))
        {
            //NO MONEY
            return;
        }

        _moneyProvider.ChangeMoney(-chip.Price);

        _chipProvider.OpenChip(chip.Index);
    }

    #region Output

    public event Action<IReadOnlyList<Chip>> OnSetChips;
    public event Action<int> OnOpenChip;

    #endregion
}
