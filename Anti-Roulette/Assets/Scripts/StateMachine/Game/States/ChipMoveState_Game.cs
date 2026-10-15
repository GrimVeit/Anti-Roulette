using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipMoveState_Game : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly IChipSpawnerListener _chipSpawnerListener;
    private readonly INumberSectorTriggerListener _numberSectorTriggerListener;

    public ChipMoveState_Game(IStateProvider stateProvider, IChipSpawnerListener chipSpawnerListener, INumberSectorTriggerListener numberSectorTriggerListener)
    {
        _stateProvider = stateProvider;
        _chipSpawnerListener = chipSpawnerListener;
        _numberSectorTriggerListener = numberSectorTriggerListener;
    }

    public void Enter()
    {
        _numberSectorTriggerListener.OnSuccess += ChangeStateToWin;
        _numberSectorTriggerListener.OnFailed += ChangeStateToLose;
        _chipSpawnerListener.OnFailed += ChangeStateToLose;
    }

    public void Exit()
    {
        _numberSectorTriggerListener.OnSuccess -= ChangeStateToWin;
        _numberSectorTriggerListener.OnFailed -= ChangeStateToLose;
        _chipSpawnerListener.OnFailed -= ChangeStateToLose;
    }

    private void ChangeStateToLose()
    {
        _stateProvider.SetState(_stateProvider.GetState<LoseState_Game>());
    }

    private void ChangeStateToWin()
    {
        _stateProvider.SetState(_stateProvider.GetState<WinState_Game>());
    }
}
