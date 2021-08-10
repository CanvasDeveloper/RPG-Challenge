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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyHit")
        {
            if(player.currentState == PlayerState.ALIVE)
            {
                MaxMinDamage shot = other.gameObject.GetComponent<MaxMinDamage>();
                player.GetHit(Random.Range(shot.minDamage, shot.maxDamage));
                if(shot.isDestructableOnCollision)
                {
                    Destroy(other.gameObject);
                }
            }
            else
            {
                Destroy(other.gameObject);
            }
            
        } 
    }
}
