using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoverHandler : MonoBehaviour, IPointerEnterHandler
{
    public event EventHandler OnHover;
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover?.Invoke(this, EventArgs.Empty);
    }    
}
