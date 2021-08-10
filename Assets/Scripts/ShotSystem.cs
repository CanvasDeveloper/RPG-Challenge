using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSystem : MonoBehaviour
{
    [SerializeField]private GameObject shotPrefab;
    [SerializeField]private float shotSpeed;
    [SerializeField]private float shotDelay;

    public void Fire(Transform shotPoint)
    {
        StartCoroutine(ShotDelay(shotPoint));
    }

    IEnumerator ShotDelay(Transform shotPoint)
    {
        yield return new WaitForSeconds(shotDelay);
        GameObject temp = Instantiate(shotPrefab, shotPoint.position, Quaternion.identity);
        temp.GetComponent<ShotScript>().SetSpeed(shotSpeed);
        temp.transform.forward = shotPoint.forward;
    }
}
