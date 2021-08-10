using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    [SerializeField]private float timeToDestroy = 4f;
    [SerializeField]private GameObject particlePrefab;
    public int maxDamage = 5;
    public int minDamage = 10;
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
        if(other.gameObject.tag != "Player" || other.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
