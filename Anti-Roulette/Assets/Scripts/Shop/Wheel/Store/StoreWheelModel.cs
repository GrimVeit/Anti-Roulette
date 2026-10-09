using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class StoreWheelModel
{
    public event Action<Wheel> OnOpenWheel;
    public event Action<Wheel> OnSelectWheel;

    private readonly Dictionary<int, Wheel> _wheels;

    private readonly string _filePath;
    private readonly string _selectedWheelKey;
    private readonly string _xorKey;

    private int _currentWheelIndex;

    public Wheel CurrentWheel => _wheels.TryGetValue(_currentWheelIndex, out var wheel) ? wheel : null;

    public StoreWheelModel(IEnumerable<WheelDataSO> data, string saveFileName = "Wheels.json", string selectedWheelKey = PlayerPrefsKeys.WHEELS, string xorKey = "eurghfuirehfisdfioerfywrewefjkowefds31uhgdg")
    {
        _wheels = new Dictionary<int, Wheel>();

        _filePath = Path.Combine(Application.persistentDataPath, saveFileName);

        _selectedWheelKey = selectedWheelKey;
        _xorKey = xorKey;

        foreach (var wheelData in data)
        {
            if (wheelData == null)
                continue;

            if (_wheels.ContainsKey(wheelData.Index))
            {
                Debug.LogError($"Duplicate wheel index: {wheelData.Index}");
                continue;
            }

            _wheels.Add(
                wheelData.Index,
                new Wheel(
                    wheelData.Index,
                    wheelData.Name,
                    wheelData.SpriteMain,
                    wheelData.SpriteShop,
                    wheelData.SpriteRoulette,
                    wheelData.SpriteCross,
                    wheelData.Price,
                    false
                )
            );
        }
    }

    #region INIT

    public void Initialize()
    {
        Load();

        int defaultIndex = GetDefaultWheelIndex();

        // Базовый фон всегда должен быть открыт.
        if (_wheels.TryGetValue(defaultIndex, out var defaultBackground))
            defaultBackground.Open();

        _currentWheelIndex = PlayerPrefs.GetInt(_selectedWheelKey, defaultIndex);

        // Если выбранный фон отсутствует — используем дефолтный.
        if (!_wheels.ContainsKey(_currentWheelIndex))
            _currentWheelIndex = defaultIndex;

        foreach (var item in _wheels)
        {
            Debug.Log($"BACKGROUND INDEX - {item.Key}, IS OPEN - {item.Value.IsOpened}");
        }
    }

    public void Dispose()
    {
        Save();

        PlayerPrefs.SetInt(_selectedWheelKey, _currentWheelIndex);
        PlayerPrefs.Save();
    }

    #endregion

    #region INPUT

    public void OpenWheel(int index)
    {
        if (!_wheels.TryGetValue(index, out var wheel))
        {
            Debug.LogError($"Wheel not found: {index}");
            return;
        }

        if (wheel.IsOpened)
            return;

        wheel.Open();

        OnOpenWheel?.Invoke(wheel);
    }

    public void SelectWheel(int index)
    {
        if (!_wheels.TryGetValue(index, out var wheel))
        {
            Debug.LogError($"Background not found: {index}");
            return;
        }

        if (!wheel.IsOpened)
            return;

        if (_currentWheelIndex == index)
            return;

        _currentWheelIndex = index;

        OnSelectWheel?.Invoke(wheel);
    }

    #endregion

    #region INFO

    public Wheel GetWheel(int index)
    {
        return _wheels.TryGetValue(index, out var background) ? background : null;
    }

    public IReadOnlyList<Wheel> GetWheels()
    {
        return _wheels.Values.OrderBy(wheel => wheel.Index).ToList();
    }

    public Wheel GetCurrentWheel()
    {
        return CurrentWheel;
    }

    public int GetCurrentWheelIndex()
    {
        return _currentWheelIndex;
    }

    public bool IsWheelOpened(int index)
    {
        return _wheels.TryGetValue(index, out var wheel) && wheel.IsOpened;
    }

    public bool IsWheelSelected(int index)
    {
        return _currentWheelIndex == index;
    }

    #endregion

    #region LOAD / SAVE

    private void Load()
    {
        if (!File.Exists(_filePath))
            return;

        try
        {
            string encrypted = File.ReadAllText(_filePath);
            string json = Xor(encrypted, _xorKey);

            var wrapper = JsonUtility.FromJson<WheelSaveWrapper>(json);

            if (wrapper?.Entries == null)
                throw new Exception("Invalid save data.");

            foreach (var entry in wrapper.Entries)
            {
                if (entry == null)
                    continue;

                if (!_wheels.TryGetValue(entry.Index, out var wheel))
                    continue;

                if (entry.IsOpened)
                    wheel.Open();
            }
        }
        catch (Exception exception)
        {
            Debug.LogError($"Failed to load wheels. Resetting to default state. {exception}");

            ResetToDefaultState();
        }
    }

    private void Save()
    {
        var wrapper = new WheelSaveWrapper();

        foreach (var wheel in _wheels.Values)
        {
            wrapper.Entries.Add(
                new WheelSaveEntry
                {
                    Index = wheel.Index,
                    IsOpened = wheel.IsOpened
                }
            );
        }

        string json = JsonUtility.ToJson(wrapper);
        string encrypted = Xor(json, _xorKey);

        File.WriteAllText(_filePath, encrypted);
    }

    private string Xor(string data, string key)
    {
        var result = new char[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            result[i] = (char)(data[i] ^ key[i % key.Length]);
        }

        return new string(result);
    }

    #endregion

    #region DEFAULT

    private int GetDefaultWheelIndex()
    {
        if (_wheels.Count == 0)
            return 0;

        return _wheels.Keys.Min();
    }

    private void ResetToDefaultState()
    {
        int defaultIndex = GetDefaultWheelIndex();

        // Закрываем абсолютно все фоны.
        foreach (var wheel in _wheels.Values)
        {
            wheel.Close();
        }

        // Открываем только базовый.
        if (_wheels.TryGetValue(defaultIndex, out var defaultWheel))
            defaultWheel.Open();

        // Выбираем базовый.
        _currentWheelIndex = defaultIndex;

        // Сбрасываем сохранённый выбор.
        PlayerPrefs.SetInt(_selectedWheelKey, defaultIndex);
        PlayerPrefs.Save();
    }

    #endregion
}

[Serializable]
public sealed class WheelSaveWrapper
{
    public List<WheelSaveEntry> Entries = new();
}

[Serializable]
public sealed class WheelSaveEntry
{
    public int Index;
    public bool IsOpened;
}
