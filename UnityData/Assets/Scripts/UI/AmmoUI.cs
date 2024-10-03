using System;
using TMPro;
using UnityEngine;

class AmmoUI : UISubsystem
{
    [Header("Inspector")]
    [SerializeField] private GameObject ammoUI;
    [SerializeField] private TextMeshProUGUI ammoText;

    int clipAmmo = 0;
    int inventoryAmmo = 0;

    public override void Bind(PlayerSubsystem playerSubsystem)
    {
        var weapon = playerSubsystem.GetComponent<PlayerActiveWeapon>();
        if (!weapon)
            return;

        weapon.OnClipAmmoChanged += OnClipAmmoChanged;
        weapon.OnInventoryAmmoChanged += OnInventoryAmmoChanged;
    }

    private void OnInventoryAmmoChanged(int obj)
    {
        inventoryAmmo = obj;
        SetAmmoText();
    }

    private void OnClipAmmoChanged(int obj)
    {
        clipAmmo = obj;
        SetAmmoText();
    }

    void SetAmmoText()
    {
        ammoUI.SetActive(clipAmmo != -1);
        ammoText.text = $"{clipAmmo}/{inventoryAmmo}";
    }
}
