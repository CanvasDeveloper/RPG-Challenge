using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine; 
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField]private Slot[] slots;
    [SerializeField]private GameObject inventoryPanel;
    [SerializeField]private GameObject btnEquip;
    [SerializeField]private GameObject detailPanel;
    [SerializeField]private Image itemImage;
    [SerializeField]private TextMeshProUGUI itemName;
    [SerializeField]private TextMeshProUGUI itemDescription;
    private Collectable currentItem;
    private void Awake()
    {
        if(Instance == null) { Instance = this; }
    }

    public void Open()
    {
        inventoryPanel.SetActive(true);
        EnableAllSlots();
        DisableEmptySlots();
        DisableDetailPanel();
        GameController.Instance.ChangeGameState(GameState.PAUSE);
    }

    public void Close()
    {
        inventoryPanel.SetActive(false);
        GameController.Instance.ChangeGameState(GameState.GAMEPLAY);
    }

    void UpdateDetailPanel()
    {
        detailPanel.SetActive(true);
        itemImage.sprite = currentItem.itemIcon;
        itemName.text = currentItem.itemName;
        itemDescription.text = currentItem.description;
        btnEquip.SetActive(!currentItem.isKeyItem);
    }

    public void DisableDetailPanel()
    {
        detailPanel.SetActive(false);
        itemImage.sprite = null;
        itemName.text = "";
        itemDescription.text = "";
        btnEquip.SetActive(false);
    }

    #region  SLOTS
    public void DisableEmptySlots()
    {
        foreach(Slot s in slots)
        {
            if(s.slotItem == null)
            {
                s.gameObject.SetActive(false);
            }
        }
    }

    public void EnableAllSlots()
    {
        foreach(Slot s in slots)
        {
            s.gameObject.SetActive(true);
        }
    }

    public void EnableKeyItensSlots()
    {
        DisableEmptySlots();

        foreach(Slot s in slots)
        {
            if(s.slotItem != null && s.slotItem.isKeyItem)
            {
                s.gameObject.SetActive(true);
            }
        }
    }

    public void EnableCollectablesSlots()
    {
        DisableEmptySlots();

        foreach(Slot s in slots)
        {
            if(s.slotItem != null && !s.slotItem.isKeyItem)
            {
                s.gameObject.SetActive(true);
            }
        }
    }
    public void UpdateSlots()
    {
        foreach(Slot s in slots)
        {
            s.LoadSlot();
        }
    }

    #endregion

    #region ITEMS
    public void GetItemInSlot(Collectable item)
    {
        currentItem = item;
        UpdateDetailPanel();
    }

    void AddItemInEmptySlot(Collectable i)
    {
        foreach(Slot s in slots)
        {
            if(s.slotItem == null)
            {
                s.currentValue += i.value;
                s.SetSlot(i);
                break;
            }
        } 
    }

    public void AddItem(Collectable item)
    {
        if(item.isKeyItem)
        {
            AddItemInEmptySlot(item);
        }
        else
        {
            bool hasItem = false;
            foreach(Slot s in slots)
            {
                if(s.slotItem == item)
                {
                    hasItem = true;
                    s.currentValue += item.value;
                    s.LoadSlot();
                    break;
                }
            }

            if(!hasItem)
            {
                AddItemInEmptySlot(item);
            }
        }
    }
    #endregion
}
