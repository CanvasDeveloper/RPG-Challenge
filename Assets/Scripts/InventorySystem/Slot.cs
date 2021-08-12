using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    [SerializeField] Image slotImage;
    [SerializeField] TextMeshProUGUI txtValue; 
    public Collectable slotItem;
    public int currentValue;

    public void LoadSlot()
    {
        slotImage.sprite = slotItem.itemIcon;
        txtValue.gameObject.SetActive(slotItem.isKeyItem);
        txtValue.text = currentValue.ToString();
    }

    public void SetSlot(Collectable item)
    {
        slotItem = item;
        LoadSlot();
    }

    public void SendItem()
    {
        Inventory.Instance.GetItemInSlot(slotItem);
    }
}
