using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string tooltip = "Tooltip name in UI.Elements";

    void OnEnable()
    {
        setTooltipState(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        setTooltipState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        setTooltipState(false);
    }

    void setTooltipState(bool state)
    {
        UI.Elements[tooltip].SetActive(state);
    }
}