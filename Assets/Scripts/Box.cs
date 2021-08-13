using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DropSystem))]
public class Box : MonoBehaviour
{
    public GameObject brokenPrefab;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerHit" || other.gameObject.tag == "PlayerMageHit" || other.gameObject.tag == "EnemyHit")
        {
            Instantiate(brokenPrefab, transform.position, Quaternion.identity);
            GetComponent<DropSystem>().Drop();
            Destroy(gameObject);
        }    
    }
}
