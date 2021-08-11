using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MaxMinDamage))]
public class ShotScript : MonoBehaviour
{
    [SerializeField]private float timeToDestroy = 4f;
    [SerializeField]private GameObject particlePrefab;
    private float speed;

    private void Start()
    {
        Destroy(this.gameObject, timeToDestroy);    
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void Update()
    {
        transform.localPosition += transform.forward * speed * Time.deltaTime;      
    }

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(particlePrefab, transform.position, Quaternion.identity);

        switch(other.gameObject.tag)
        {
            case "Player":
            break;

            case "PlayerMageHit":
            break;

            case "Enemy":
            break;

            default:
                Destroy(this.gameObject);
            break;

        }
    }
}
