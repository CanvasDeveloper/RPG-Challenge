using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image slotImage;
    [SerializeField] GameObject slotEquippedIcon;
    [SerializeField] Image slotSelectedImage;
    [SerializeField] TextMeshProUGUI txtValue; 
    public Collectable slotItem;
    public int currentValue;

    public void OnSelect(BaseEventData eventData)
    {
        slotSelectedImage.gameObject.SetActive(true);
        Inventory.Instance.GetItemInSlot(slotItem);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Inventory.Instance.DisableDetailPanel();
        slotSelectedImage.gameObject.SetActive(false);
    }

    public void LoadSlot()
    {
        if(slotItem == null) { return; }
        slotEquippedIcon.SetActive(slotItem.isEquipped);
        slotImage.sprite = slotItem.itemIcon;
        txtValue.gameObject.SetActive(!slotItem.isKeyItem);
        txtValue.text = currentValue.ToString();
    }

    public void SetSlot(Collectable item)
    {
        slotItem = item;
        LoadSlot();
    }

    public void ResetSlot()
    {
        slotItem = null;
        slotImage.sprite = null;
        txtValue.text = "";
        currentValue = 0;
    }


    public void EquipItem()
    {
        if(!slotItem.isKeyItem)
        {
            print("o");
            Inventory.Instance.EquipItem();
        }
    }
}
