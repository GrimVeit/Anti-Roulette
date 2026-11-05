using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public sealed class SupabasePlayerDatabasePresenter : IPlayerDatabaseProvider
{
    private readonly SupabasePlayerDatabaseModel _model;

    public SupabasePlayerDatabasePresenter(SupabasePlayerDatabaseModel model)
    {
        _model = model;
    }

    public void Cancel() => _model.Dispose();

    public UniTask<(DatabaseResult Result, PlayerData Data)> LoadPlayer() => _model.LoadPlayer();

    public UniTask<DatabaseResult> CreateOrUpdatePlayer(PlayerData data) => _model.CreateOrUpdatePlayer(data);
}

public interface IPlayerDatabaseProvider
{
    UniTask<(DatabaseResult Result, PlayerData Data)> LoadPlayer();
    UniTask<DatabaseResult> CreateOrUpdatePlayer(PlayerData data);
}
