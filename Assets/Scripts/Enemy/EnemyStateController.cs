using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    PATROL, ATTACK, DEAD
}

public class EnemyStateController : MonoBehaviour
{
    public EnemyState currentState;
}
