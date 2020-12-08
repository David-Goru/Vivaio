using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ISlot
{
    [SerializeField] public string SlotName;
    [SerializeField] public string ButtonName;
    [System.NonSerialized] public GameObject Slot;
    [System.NonSerialized] public GameObject Button;

    public ISlot(string buttonName, GameObject slot)
    {
        ButtonName = buttonName;
        Slot = slot;
        SlotName = slot.name;
    }

    public void Initialize()
    {
        Button = GameObject.Find("UI").transform.Find("Top buttons bar").Find("Buttons").Find(ButtonName).gameObject;
        Button.GetComponent<RectTransform>().anchoredPosition = Slot.GetComponent<RectTransform>().anchoredPosition;
    }

    public void UpdatePosition()
    {
        if (Button == null) return;
        Button.GetComponent<RectTransform>().anchoredPosition = Slot.GetComponent<RectTransform>().anchoredPosition;
    }

    public void SetButton(GameObject button)
    {
        Button = button;
        ButtonName = button.name;
        UpdatePosition();
    }
}