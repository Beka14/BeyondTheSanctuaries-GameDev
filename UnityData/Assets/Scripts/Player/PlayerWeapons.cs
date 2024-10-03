using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : PlayerSubsystem
{
    [SerializeField] List<GameObject> weapons = new();
    [SerializeField] Transform weaponPivot;
    [SerializeField] Transform weaponsTransform;

    private void Start()
    {
        EquipWeapon(1);
    }
    private void OnEnable()
    {
        Controls.OnNumberPressed += EquipWeapon;
        player.events.OnWeaponUnequipped += OnWeaponUnequipped;
    }
    private void OnDisable()
    {
        Controls.OnNumberPressed -= EquipWeapon;
        player.events.OnWeaponUnequipped -= OnWeaponUnequipped;
    }

    private void OnWeaponUnequipped(GameObject @object)
    {
        @object.transform.SetParent(weaponsTransform);
    }


    private void EquipWeapon(int obj)
    {
        obj -= 1;
        if (obj < weapons.Count)
        {
            weapons[obj].transform.SetParent(weaponPivot);
            player.events.OnWeaponEquipped?.Invoke(weapons[obj]);
        }
    }
}