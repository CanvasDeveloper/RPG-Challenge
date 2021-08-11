using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IHealthSystem
{
    void Death();
    void GetHit(int damage);
}

//=== FOR SECURITY IMPLEMENT IHEALTHSYSTEM IN YOUR MAIN SCRIPT===
public class HealthSystem : MonoBehaviour
{
    [SerializeField]private float maxHealth;
    [SerializeField]private Transform spawnLetter;
    [SerializeField]private GameObject damageTxt;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            gameObject.SendMessage("Death", SendMessageOptions.RequireReceiver);
        }
        else
        {
            GameObject temp = Instantiate(damageTxt, spawnLetter.position, Quaternion.identity);
            temp.GetComponent<TMP_Text>().text = damage.ToString();
        }
    }

    public void RecoveryHealth(int value)
    {
        currentHealth += value;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
