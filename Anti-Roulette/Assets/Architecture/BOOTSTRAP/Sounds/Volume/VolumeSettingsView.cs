using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsView : View
{
    [SerializeField] private VolumeSetting soundVolume;
    [SerializeField] private VolumeSetting musicVolume;

    public void Initialize()
    {
        soundVolume.OnChangeMute += HandleSoundVolumeChanged;
        musicVolume.OnChangeMute += HandleMusicVolumeChanged;

        soundVolume.Initialize();
        musicVolume.Initialize();
    }

    public void Dispose()
    {
        soundVolume.OnChangeMute -= HandleSoundVolumeChanged;
        musicVolume.OnChangeMute -= HandleMusicVolumeChanged;

        soundVolume.Dispose();
        musicVolume.Dispose();
    }

    public void SetSoundVolume(bool value)
    {
        soundVolume.SetValue(value);
    }

    public void SetMusicVolume(bool value)
    {
        musicVolume.SetValue(value);
    }

    private void HandleSoundVolumeChanged(bool value)
    {
        OnChangeSoundMute?.Invoke(value);
    }

    private void HandleMusicVolumeChanged(bool value)
    {
        OnChangeMusicMute?.Invoke(value);
    }

    #region Output

    public event Action<bool> OnChangeSoundMute;
    public event Action<bool> OnChangeMusicMute;

    #endregion



    [Serializable]
    private class VolumeSetting
    {
        [SerializeField] private Button buttonOn;
        [SerializeField] private Button buttonOff;

        [SerializeField] private RectTransform fillOn;
        [SerializeField] private RectTransform fillOff;

        [SerializeField] private float duration = 0.2f;
        [SerializeField] private Ease ease = Ease.OutQuad;

        private const float MinWidth = 0f;
        private const float MaxWidth = 150f;

        private Sequence _sequence;

        public void Initialize()
        {
            buttonOn.onClick.AddListener(ClickOn);
            buttonOff.onClick.AddListener(ClickOff);
        }

        public void Dispose()
        {
            buttonOn.onClick.RemoveListener(ClickOn);
            buttonOff.onClick.RemoveListener(ClickOff);
            _sequence?.Kill();
        }

        public void SetValue(bool isMute)
        {
            _sequence?.Kill();

            // —тартовое состо€ние: у активного Ч максимум, у неактивного Ч минимум
            if (isMute)
            {
                SetWidth(fillOff, MaxWidth);
                SetWidth(fillOn, MinWidth);
            }
            else
            {
                SetWidth(fillOn, MaxWidth);
                SetWidth(fillOff, MinWidth);
            }

            _sequence = DOTween.Sequence();

            if (isMute)
            {
                _sequence
                    .Append(fillOff.DOSizeDelta(new Vector2(MinWidth, fillOff.sizeDelta.y), duration).SetEase(ease))
                    .Append(fillOn.DOSizeDelta(new Vector2(MaxWidth, fillOn.sizeDelta.y), duration).SetEase(ease));
            }
            else
            {
                _sequence
                    .Append(fillOn.DOSizeDelta(new Vector2(MinWidth, fillOn.sizeDelta.y), duration).SetEase(ease))
                    .Append(fillOff.DOSizeDelta(new Vector2(MaxWidth, fillOff.sizeDelta.y), duration).SetEase(ease));
            }
        }

        private static void SetWidth(RectTransform rt, float width)
        {
            rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
        }

        private void ClickOn()
        {
            OnChangeMute?.Invoke(true);
        }

        private void ClickOff()
        {
            OnChangeMute?.Invoke(false);
        }

        public event Action<bool> OnChangeMute;
    }
}
