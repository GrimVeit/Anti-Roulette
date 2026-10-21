using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFinishState_Game : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly IChipCounterInfo _chipCounterInfo;

    public CheckFinishState_Game(IStateProvider stateProvider, IChipCounterInfo chipCounterInfo)
    {
        _stateProvider = stateProvider;
        _chipCounterInfo = chipCounterInfo;
    }

    public void Enter()
    {
        if(_chipCounterInfo.Count > 0)
        {
            ChangeStateToContinue();
            return;
        }

        ChangeStateToLose();
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

    }

    private void ChangeStateToLose()
    {
        //_stateProvider.SetState(_stateProvider.GetState<>());
    }
}
