using System.Threading;
using Cysharp.Threading.Tasks;

public class RouletteState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly IStoreNumberSectorProvider _storeNumberSectorProvider;

    public RouletteState_Game(IStateProvider stateProvider, IStoreNumberSectorProvider storeNumberSectorProvider)
    {
        _stateProvider = stateProvider;
        _storeNumberSectorProvider = storeNumberSectorProvider;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _storeNumberSectorProvider.ChooseRandom();

        await UniTask.WaitForSeconds(1f, cancellationToken: token);

        ChangeStateToNumberPresentation();

        await UniTask.CompletedTask;
    }

    private void ChangeStateToNumberPresentation()
    {
        _stateProvider.SetState(_stateProvider.GetState<NumberPresentationState_Game>());
    }
}
