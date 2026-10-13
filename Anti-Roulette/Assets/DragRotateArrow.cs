using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragRotateArrow : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("—сылки")]
    [Tooltip("RectTransform стрелки (если она дочерний объект круга)")]
    public RectTransform targetTransform;

    [Header("Ќастройки вращени€")]
    [Tooltip("—мещение, если стрелка по умолчанию смотрит не вправо")]
    public float angleOffset = 0f;

    [Tooltip("—корость сглаживани€. „ем больше Ч тем резче. 0 = без сглаживани€")]
    [Range(0f, 30f)]
    public float rotationSmoothSpeed = 12f;

    private RectTransform rectTransform;
    private float currentAngle;      // фактический угол стрелки (дл€ smooth)
    private float targetAngle;       // угол, к которому стремимс€
    private bool isDragging;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (targetTransform == null)
            targetTransform = rectTransform;

        // «апоминаем текущий угол как стартовый
        currentAngle = targetTransform.localEulerAngles.z - angleOffset;
        targetAngle = currentAngle;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // —разу задаЄм цель Ч направление на палец
        targetAngle = GetPointerAngle(eventData);
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ѕалец относительно центра всегда задаЄт направление стрелки
        targetAngle = GetPointerAngle(eventData);
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        // targetAngle остаЄтс€ последним Ч стрелка там и останетс€
    }

    void Update()
    {
        if (rotationSmoothSpeed <= 0f)
        {
            currentAngle = targetAngle;
        }
        else
        {
            // —глаживание через Lerp с учЄтом кратчайшего пути (важно дл€ углов!)
            currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * rotationSmoothSpeed);
        }

        targetTransform.localRotation = Quaternion.Euler(0f, 0f, currentAngle + angleOffset);
    }

    private float GetPointerAngle(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        return Mathf.Atan2(localPoint.y, localPoint.x) * Mathf.Rad2Deg;
    }
}