using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField]private Collectable item;
    
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Player")
       {
           Inventory.Instance.AddItem(item);
           if(item == GameController.Instance.cristalFireItem)
           {
               other.GetComponent<PlayerController>().ActivePower();
           }
           Destroy(this.gameObject);
       }
    }
}
