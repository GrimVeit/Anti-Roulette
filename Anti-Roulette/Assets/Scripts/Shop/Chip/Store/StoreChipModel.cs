using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class StoreChipModel
{
    public event Action<Chip> OnOpenChip;
    public event Action<Chip> OnSelectChip;

    private readonly Dictionary<int, Chip> _chips;

    private readonly string _filePath;
    private readonly string _selectedChipKey;
    private readonly string _xorKey;

    private int _currentChipIndex;

    public Chip CurrentChip => _chips.TryGetValue(_currentChipIndex, out var chip) ? chip : null;

    public StoreChipModel(IEnumerable<ChipDataSO> data, string saveFileName = "Chips.json", string selectedBackgroundKey = PlayerPrefsKeys.CHIPS, string xorKey = "eurghfuirehfisdfioerfywre49267194744uhgdg")
    {
        _chips = new Dictionary<int, Chip>();

        _filePath = Path.Combine(Application.persistentDataPath, saveFileName);

        _selectedChipKey = selectedBackgroundKey;
        _xorKey = xorKey;

        foreach (var backgroundData in data)
        {
            if (backgroundData == null)
                continue;

            if (_chips.ContainsKey(backgroundData.Index))
            {
                Debug.LogError($"Duplicate chip index: {backgroundData.Index}");
                continue;
            }

            _chips.Add(
                backgroundData.Index,
                new Chip(
                    backgroundData.Index,
                    backgroundData.Name,
                    backgroundData.SpriteShop,
                    backgroundData.Sprite,
                    backgroundData.Price,
                    false
                )
            );
        }
    }

    #region INIT

    public void Initialize()
    {
        Load();

        int defaultIndex = GetDefaultChipIndex();

        // Базовый фон всегда должен быть открыт.
        if (_chips.TryGetValue(defaultIndex, out var defaultChip))
            defaultChip.Open();

        _currentChipIndex = PlayerPrefs.GetInt(_selectedChipKey, defaultIndex);

        // Если выбранный фон отсутствует — используем дефолтный.
        if (!_chips.ContainsKey(_currentChipIndex))
            _currentChipIndex = defaultIndex;

        if (_chips.TryGetValue(_currentChipIndex, out Chip background))
        {
            if (!background.IsOpened)
                _currentChipIndex = defaultIndex;
        }

        foreach (var item in _chips)
        {
            Debug.Log($"CHIP INDEX - {item.Key}, IS OPEN - {item.Value.IsOpened}");
        }
    }

    public void Dispose()
    {
        Save();

        PlayerPrefs.SetInt(_selectedChipKey, _currentChipIndex);
        PlayerPrefs.Save();
    }

    #endregion

    #region INPUT

    public void OpenChip(int index)
    {
        if (!_chips.TryGetValue(index, out var chip))
        {
            Debug.LogError($"Chip not found: {index}");
            return;
        }

        if (chip.IsOpened)
            return;

        chip.Open();

        OnOpenChip?.Invoke(chip);
    }

    public void SelectChip(int index)
    {
        if (!_chips.TryGetValue(index, out var chip))
        {
            Debug.LogError($"Background not found: {index}");
            return;
        }

        if (!chip.IsOpened)
            return;

        if (_currentChipIndex == index)
            return;

        _currentChipIndex = index;

        OnSelectChip?.Invoke(chip);
    }

    #endregion

    #region INFO

    public Chip GetChip(int index)
    {
        return _chips.TryGetValue(index, out var chip) ? chip : null;
    }

    public IReadOnlyList<Chip> GetChips()
    {
        return _chips.Values
            .OrderBy(chip => chip.Index)
            .ToList();
    }

    public Chip GetCurrentChip()
    {
        return CurrentChip;
    }

    public int GetCurrentChipIndex()
    {
        return _currentChipIndex;
    }

    public bool IsChipOpened(int index)
    {
        return _chips.TryGetValue(index, out var chip) && chip.IsOpened;
    }

    public bool IsChipSelected(int index)
    {
        return _currentChipIndex == index;
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

            var wrapper = JsonUtility.FromJson<ChipSaveWrapper>(json);

            if (wrapper?.Entries == null)
                throw new Exception("Invalid save data.");

            foreach (var entry in wrapper.Entries)
            {
                if (entry == null)
                    continue;

                if (!_chips.TryGetValue(entry.Index, out var chip))
                    continue;

                if (entry.IsOpened)
                    chip.Open();
            }
        }
        catch (Exception exception)
        {
            Debug.LogError($"Failed to load chips. Resetting to default state. {exception}");

            ResetToDefaultState();
        }
    }

    private void Save()
    {
        var wrapper = new ChipSaveWrapper();

        foreach (var chip in _chips.Values)
        {
            wrapper.Entries.Add(
                new ChipSaveEntry
                {
                    Index = chip.Index,
                    IsOpened = chip.IsOpened
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

    private int GetDefaultChipIndex()
    {
        if (_chips.Count == 0)
            return 0;

        return _chips.Keys.Min();
    }

    private void ResetToDefaultState()
    {
        int defaultIndex = GetDefaultChipIndex();

        // Закрываем абсолютно все фоны.
        foreach (var chip in _chips.Values)
        {
            chip.Close();
        }

        // Открываем только базовый.
        if (_chips.TryGetValue(defaultIndex, out var defaultChip))
            defaultChip.Open();

        // Выбираем базовый.
        _currentChipIndex = defaultIndex;

        // Сбрасываем сохранённый выбор.
        PlayerPrefs.SetInt(_selectedChipKey, defaultIndex);
        PlayerPrefs.Save();
    }

    #endregion

}

[Serializable]
public sealed class ChipSaveWrapper
{
    public List<ChipSaveEntry> Entries = new();
}

[Serializable]
public sealed class ChipSaveEntry
{
    public int Index;
    public bool IsOpened;
}
