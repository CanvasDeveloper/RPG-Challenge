using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(40f, 50f, 0f));
        Destroy(gameObject, 4f);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);    
    }
}
