using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChipSliderView : View
{
    [Header("UI")]
    public Slider powerSlider;          // вертикальный слайдер
    public TextMeshProUGUI powerText;   // текст силы

    [Header("Настройки")]
    public float returnSpeed = 3f;      // скорость возврата
    public float maxPower = 100f;       // макс. сила (когда слайдер = 1)

    private bool isDragging = false;
    private Coroutine returnRoutine;

    private bool isActive = false;

    public void Initialize()
    {
        powerSlider.minValue = 0f;
        powerSlider.maxValue = 1f;
        powerSlider.value = 0f;
        powerSlider.direction = Slider.Direction.RightToLeft; // 0 внизу, 1 вверху

        // Подписки на события EventTrigger для определения нажатия/отпускания
        var trigger = powerSlider.gameObject.AddComponent<EventTrigger>();

        var down = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        down.callback.AddListener(_ => OnPointerDown());
        trigger.triggers.Add(down);

        var up = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        up.callback.AddListener(_ => OnPointerUp());
        trigger.triggers.Add(up);

        powerSlider.onValueChanged.AddListener(UpdateText);
        UpdateText(0f);
    }

    public void Activate()
    {
        isActive = true;
        powerSlider.interactable = true;
    }

    public void Deactivate()
    {
        isActive = false;
        powerSlider.interactable = false;

        if (returnRoutine != null) StopCoroutine(returnRoutine);
        returnRoutine = StartCoroutine(ReturnToZero());
    }

    private void OnPointerDown()
    {
        if(!isActive) return;

        if (returnRoutine != null) StopCoroutine(returnRoutine);
        isDragging = true;
    }

    private void OnPointerUp()
    {
        if (!isActive) return;

        if (!isDragging) return;
        isDragging = false;

        float shotPower = powerSlider.value * maxPower;
        Debug.Log($"Выстрел силой {shotPower}");

        if(shotPower > 0.01f)
            OnShotPower?.Invoke(shotPower);

        // тут запускай сам выстрел: shotPower

        if (returnRoutine != null) StopCoroutine(returnRoutine);
        returnRoutine = StartCoroutine(ReturnToZero());
    }

    private void UpdateText(float v)
    {
        float power = v * maxPower;
        powerText.text = Mathf.RoundToInt(power).ToString();
    }

    IEnumerator ReturnToZero()
    {
        while (powerSlider.value > 0.001f)
        {
            float newValue = Mathf.MoveTowards(
                powerSlider.value, 0f, returnSpeed * Time.deltaTime);
            powerSlider.SetValueWithoutNotify(newValue);
            UpdateText(newValue);
            yield return null;
        }
        powerSlider.SetValueWithoutNotify(0f);
        UpdateText(0f);
    }

    #region Output

    public event Action<float> OnShotPower;

    #endregion
}
