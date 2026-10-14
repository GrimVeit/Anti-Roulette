using System.Threading;
using Cysharp.Threading.Tasks;

public class RouletteState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly IStoreNumberSectorProvider _storeNumberSectorProvider;

    public RouletteState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, IStoreNumberSectorProvider storeNumberSectorProvider)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _storeNumberSectorProvider = storeNumberSectorProvider;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowRoulettePanel();

        _storeNumberSectorProvider.ChooseRandom();

        await UniTask.WaitForSeconds(2f, cancellationToken: token);

        ChangeStateToNumberPresentation();

        await UniTask.CompletedTask;
    }

    private void ChangeStateToNumberPresentation()
    {
        _stateProvider.SetState(_stateProvider.GetState<NumberPresentationState_Game>());
    }
}
