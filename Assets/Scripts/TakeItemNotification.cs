using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TakeItemNotification : MonoBehaviour
{
    [SerializeField]private Image itemIcon;
    [SerializeField]private TextMeshProUGUI txtItemName;

    private void Start() {
        Destroy(gameObject, 2.5f);
    }

    public void UpdateNotification(Sprite itemSprite, string itemName)
    {
        itemIcon.sprite = itemSprite;
        txtItemName.text = itemName;
    }
}
