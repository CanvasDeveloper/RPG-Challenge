using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MaxMinDamage))]
public class LaserFire : MonoBehaviour
{   
    private SphereCollider sphereCollider;
    [SerializeField]private float timeActive = 4f;
    [SerializeField]private float times = 3f;
    [SerializeField]private float timeBetweenTimes = 0.8f;

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        StartCoroutine(DamageDelay());
        Destroy(this.gameObject, timeActive);
    }

    IEnumerator DamageDelay()
    {
        for(int i = 0; i <= times; i++)
        {
            sphereCollider.enabled = true;
            yield return new WaitForSeconds(timeBetweenTimes);
            sphereCollider.enabled = false;
        }
    }
}
