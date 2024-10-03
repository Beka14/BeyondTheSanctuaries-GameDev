using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Inventory System/Items/Weapon")]
public class WeaponObject : ItemObject
{
    public Weapon weapon;
    
    public void Awake()
    {
        type = ItemType.Weapon;
    }
}