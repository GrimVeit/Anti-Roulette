using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChipArrowView : View
{
    public Vector2 Direction
    {
        get
        {
            float rad = (currentAngle + angleOffset) * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }
    }
    public float CurrentAngle => currentAngle + angleOffset;

    [Header("Ссылки")]
    [Tooltip("RectTransform стрелки (если она дочерний объект круга)")]
    public RectTransform targetTransform;

    [Header("Основа")]
    [SerializeField] private RectTransform transformMain;
    [SerializeField] private float durationActivateDeactivate;

    [Header("Настройки вращения")]
    [Tooltip("Смещение, если стрелка по умолчанию смотрит не вправо")]
    public float angleOffset = 0f;

    [Tooltip("Скорость сглаживания. Чем больше — тем резче. 0 = без сглаживания")]
    [Range(0f, 30f)]
    public float rotationSmoothSpeed = 12f;

    private RectTransform rectTransform;
    private float currentAngle;      // фактический угол стрелки (для smooth)
    private float targetAngle;       // угол, к которому стремимся
    private bool isDragging;

    private bool isActive = false;
    private Tween tweenScale;

    public void Initialize()
    {
        rectTransform = GetComponent<RectTransform>();

        if (targetTransform == null)
            targetTransform = rectTransform;

        // Запоминаем текущий угол как стартовый
        currentAngle = targetTransform.localEulerAngles.z - angleOffset;
        targetAngle = currentAngle;
    }

    void Update()
    {
        if (rotationSmoothSpeed <= 0f)
        {
            currentAngle = targetAngle;
        }
        else
        {
            // Сглаживание через Lerp с учётом кратчайшего пути (важно для углов!)
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

    public void Activate()
    {
        tweenScale?.Kill();

        isActive = true;

        tweenScale = transformMain.DOScale(1, durationActivateDeactivate);
    }

    public void Deactivate()
    {
        tweenScale?.Kill();

        isActive = false;

        tweenScale = transformMain.DOScale(0, durationActivateDeactivate);
    }

    #region Interface

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!isActive) return;

        targetAngle = GetPointerAngle(eventData);
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isActive) return;

        targetAngle = GetPointerAngle(eventData);
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    #endregion
}
