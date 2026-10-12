using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SlingshotSlider : MonoBehaviour
{
    [Header("UI")]
    public Slider powerSlider;          // вертикальный слайдер
    public TextMeshProUGUI powerText;   // текст силы

    [Header("Настройки")]
    public float returnSpeed = 3f;      // скорость возврата
    public float maxPower = 100f;       // макс. сила (когда слайдер = 1)

    private bool isDragging = false;
    private Coroutine returnRoutine;

    void Start()
    {
        // Слайдер вертикальный: сверху 1, снизу 0
        powerSlider.minValue = 0f;
        powerSlider.maxValue = 1f;
        powerSlider.value = 0f;
        powerSlider.direction = Slider.Direction.RightToLeft; // 0 внизу, 1 вверху

        // Подписки на события EventTrigger для определения нажатия/отпускания
        var trigger = powerSlider.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();

        var down = new UnityEngine.EventSystems.EventTrigger.Entry
        { eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown };
        down.callback.AddListener(_ => OnPointerDown());
        trigger.triggers.Add(down);

        var up = new UnityEngine.EventSystems.EventTrigger.Entry
        { eventID = UnityEngine.EventSystems.EventTriggerType.PointerUp };
        up.callback.AddListener(_ => OnPointerUp());
        trigger.triggers.Add(up);

        powerSlider.onValueChanged.AddListener(UpdateText);
        UpdateText(0f);
    }

    void OnPointerDown()
    {
        if (returnRoutine != null) StopCoroutine(returnRoutine);
        isDragging = true;
    }

    void OnPointerUp()
    {
        if (!isDragging) return;
        isDragging = false;

        float shotPower = powerSlider.value * maxPower;
        Debug.Log($"Выстрел силой {shotPower}");

        // тут запускай сам выстрел: shotPower

        if (returnRoutine != null) StopCoroutine(returnRoutine);
        returnRoutine = StartCoroutine(ReturnToZero());
    }

    void UpdateText(float v)
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
}
