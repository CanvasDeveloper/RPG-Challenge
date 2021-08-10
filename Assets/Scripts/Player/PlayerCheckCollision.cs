using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckCollision : MonoBehaviour
{
    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "EnemyHit")
        {
            ShotScript shot = other.gameObject.GetComponent<ShotScript>();
            player.GetHit(Random.Range(shot.minDamage, shot.maxDamage));
            Destroy(other.gameObject);
        }    
    }
}
