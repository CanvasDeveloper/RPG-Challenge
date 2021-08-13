using UnityEngine;

public class DropSystem : MonoBehaviour
{
    [SerializeField]private GameObject[] dropItensPrefab;
    [SerializeField]private Transform dropPoint;
    [SerializeField]private float dropChance;

    public void Drop()
    {
        if(Random.Range(0f, 100f) <= dropChance)
        {
            Instantiate(dropItensPrefab[Random.Range(0, dropItensPrefab.Length)], dropPoint.position, Quaternion.identity);
        }
    }
}
