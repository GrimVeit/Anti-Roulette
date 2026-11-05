using System.IO;
using Newtonsoft.Json;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using UnityEngine;

public sealed class FileSessionHandler : IGotrueSessionPersistence<Session>
{
    private readonly string _path;

    public FileSessionHandler()
    {
        _path = Path.Combine(Application.persistentDataPath, "supabase_session.json");
    }

    public void SaveSession(Session session)
    {
        if (session == null) return;
        File.WriteAllText(_path, JsonConvert.SerializeObject(session));
        Debug.Log($"[Session] saved to {_path}");
    }

    public void DestroySession()
    {
        if (File.Exists(_path)) File.Delete(_path);
        Debug.Log("[Session] destroyed");
    }

    public Session LoadSession()
    {
        if (!File.Exists(_path)) return null;
        var json = File.ReadAllText(_path);
        if (string.IsNullOrEmpty(json)) return null;
        Debug.Log("[Session] loaded from file");
        return JsonConvert.DeserializeObject<Session>(json);
    }
}
