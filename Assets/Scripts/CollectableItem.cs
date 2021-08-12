using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField]private Collectable item = new Collectable();
    
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Player")
       {
           Inventory.Instance.AddItem(item);
           //Destroy(this.gameObject);
       }
    }
}
