using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    IDLE, PATROL, CHASE, ATTACK, VICTORY, DEAD
}

public class EnemyStateController : MonoBehaviour
{
    public EnemyState currentState;
}
