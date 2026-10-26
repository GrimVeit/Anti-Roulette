using System;
using UnityEngine;

public class ChipSliderModel
{
    // ƒиапазоны настройки звука
    private const float MinVolume = 0.25f;
    private const float MaxVolume = 1.0f;

    private const float MinPitch = 0.85f;
    private const float MaxPitch = 1.15f;

    // ≈сли value == 0 Ч выстрела нет, звук не играем
    private const float FireThreshold = 0.001f;

    private readonly ISoundProvider _soundProvider;
    private readonly ISound _soundShot;

    private float _lastFire = -1f;

    public ChipSliderModel(ISoundProvider soundProvider)
    {
        _soundProvider = soundProvider;
        _soundShot = _soundProvider.GetSound("Chip_Fire");
    }

    public void SetFire(float value)
    {
        value = Mathf.Clamp01(value);

        // 0.0 Ч вообще нет выстрела
        if (value < FireThreshold)
        {
            _lastFire = 0f;
            return;
        }

        // Ќе дЄргаем звук повторно на то же значение (если SetFire вызываетс€ каждый кадр)
        if (Mathf.Approximately(value, _lastFire))
            return;

        _lastFire = value;

        // Ћинейна€ интерпол€ци€. ѕри желании замените на кривую:
        // float t = value * value; // или Mathf.Sqrt(value) Ч на вкус
        float t = value;

        float volume = Mathf.Lerp(MinVolume, MaxVolume, t);
        float pitch = Mathf.Lerp(MinPitch, MaxPitch, t);

        _soundShot.SetVolume(volume);
        _soundShot.SetPitch(pitch);
        _soundShot.PlayOneShot(); // или Play() Ч зависит от того, как устроен провайдер
    }

    public void Activate() => OnActivate?.Invoke();
    public void Deactivate() => OnDeactivate?.Invoke();

    #region Output
    public event Action OnActivate;
    public event Action OnDeactivate;
    #endregion
}
