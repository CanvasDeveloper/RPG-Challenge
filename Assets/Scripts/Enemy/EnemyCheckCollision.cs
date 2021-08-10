using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheckCollision : MonoBehaviour
{
    private EnemyController enemy;

    private void Start()
    {
        enemy = GetComponent<EnemyController>();
    }

    private void OnTriggerEnter(Collider other) {
        print(other.gameObject.name);
        if(other.gameObject.tag == "PlayerMageHit")
        {
            MaxMinDamage shot = other.gameObject.GetComponent<MaxMinDamage>();
            enemy.GetHit(Random.Range(shot.minDamage, shot.maxDamage));
            if(shot.isDestructableOnCollision)
            {
                Destroy(other.gameObject);
            }
        }
    }
}

