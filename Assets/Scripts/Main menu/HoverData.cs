using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ObjectToShow;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ObjectToShow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ObjectToShow.SetActive(false);
    }
}