using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Collectable", fileName = "new collectable")]
public class Collectable : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;
    public int value;
}
