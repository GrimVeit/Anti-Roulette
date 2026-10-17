using UnityEngine;
using System;
using DG.Tweening;

public class RouletteSpinView : View
{
    [Header("Настройки вращения")]
    [SerializeField] private int minSpins = 10;
    [SerializeField] private int maxSpins = 15;

    [Tooltip("Ease для затухания. Для казино: OutSine / OutCubic")]
    [SerializeField] private Ease easeType = Ease.OutSine;

    [Header("Ссылки")]
    [SerializeField] private Transform wheelTransform;

    public event Action OnSpinStopped;

    private bool _isSpinning;
    private Tween _spinTween;
    private float _currentAngle;

    public bool IsSpinning => _isSpinning;

    public void Spin(float spinDuration)
    {
        if (_isSpinning) return;

        _isSpinning = true;

        int fullSpins = UnityEngine.Random.Range(minSpins, maxSpins + 1);
        float randomAngle = UnityEngine.Random.Range(0f, 360f);

        float startAngle = _currentAngle;
        // Минус — вращение по часовой стрелке
        float targetAngle = startAngle - (fullSpins * 360f + randomAngle);

        _spinTween?.Kill();

        _spinTween = DOTween.To(
                () => _currentAngle,
                x =>
                {
                    _currentAngle = x;
                    wheelTransform.localEulerAngles = new Vector3(0f, 0f, x);
                },
                targetAngle,
                spinDuration
            )
            .SetEase(easeType)
            .OnComplete(() =>
            {
                _currentAngle = targetAngle;
                _isSpinning = false;
                OnSpinStopped?.Invoke();
            });
    }

    public void StopImmediately()
    {
        if (!_isSpinning) return;
        _spinTween?.Kill();
        _isSpinning = false;
        OnSpinStopped?.Invoke();
    }

    private void OnDestroy()
    {
        _spinTween?.Kill();
    }
}