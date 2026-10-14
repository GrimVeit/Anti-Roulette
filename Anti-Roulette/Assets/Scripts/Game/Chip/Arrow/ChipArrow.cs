using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChipArrow : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnMove?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnUp?.Invoke(eventData);
    }

    #region Ouput

    public event Action<PointerEventData> OnDown;
    public event Action<PointerEventData> OnUp;
    public event Action<PointerEventData> OnMove;

    #endregion
}
