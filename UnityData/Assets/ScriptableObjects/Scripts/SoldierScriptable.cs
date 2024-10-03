using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Soldier", menuName = "Enemy/Soldier")]
public class SoldierScriptable : ScriptableObject
{
    public float maxHealth;
    public float currentHealth;
    public float runSpeed;
    public float walkSpeed;
}
