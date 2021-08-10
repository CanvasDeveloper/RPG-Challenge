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
            MaxMinDamage shot = other.gameObject.GetComponent<MaxMinDamage>();
            player.GetHit(Random.Range(shot.minDamage, shot.maxDamage));
            Destroy(other.gameObject);
        }    
    }
}
