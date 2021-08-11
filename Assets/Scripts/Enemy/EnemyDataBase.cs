using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataBase : MonoBehaviour
{
    public static EnemyDataBase Instance;
    public PlayerController player;

    private void Awake()
    {
        if(Instance == null) { Instance = this;}
        else { Destroy(gameObject); }    
    }

    private void Start()
    {
        player = FindObjectOfType(typeof(PlayerController)) as PlayerController;    
    }
}
