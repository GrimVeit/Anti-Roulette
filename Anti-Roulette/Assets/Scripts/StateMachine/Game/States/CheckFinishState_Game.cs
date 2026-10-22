using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFinishState_Game : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly IChipCounterInfo _chipCounterInfo;
    private readonly IFinishInfo _finishInfo;

    public CheckFinishState_Game(IStateProvider stateProvider, IChipCounterInfo chipCounterInfo, IFinishInfo finishInfo)
    {
        _stateProvider = stateProvider;
        _chipCounterInfo = chipCounterInfo;
        _finishInfo = finishInfo;
    }

    public void Enter()
    {
        if(_chipCounterInfo.Count > 0)
        {
            ChangeStateToContinue();
            return;
        }

        if (_finishInfo.IsWin)
        {
            ChangeStateToWin();
        }
        else
        {
            ChangeStateToLose();
        }
    }

    public void Exit()
    {

    }

    private void ChangeStateToContinue()
    {
        _stateProvider.SetState(_stateProvider.GetState<HoldClickRouletteState_Game>());
    }

    private void ChangeStateToWin()
    {
        _stateProvider.SetState(_stateProvider.GetState<WinState_Game>());
    }

    private void ChangeStateToLose()
    {
        _stateProvider.SetState(_stateProvider.GetState<LoseState_Game>());
    }
}
