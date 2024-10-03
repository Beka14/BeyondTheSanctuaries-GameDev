using System;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class PlayerActiveWeapon : PlayerSubsystem
{
    [Header("References")]
    [SerializeField] Animator animator;

    [Header("Weapon")]
    [SerializeField] AbstractWeaponController weapon;

    [Header("Animation Callbacks")]
    [SerializeField] AnimEvent callback;

    // Events
    public Action<int> OnClipAmmoChanged;
    public Action<int> OnInventoryAmmoChanged;

    // Aim
    bool aimBlock = false;
    bool isSprinting = false;
    bool block = false;

    InventoryComponent inventory;

    void Block()
    {
        StopSprint();
        block = true;
        aimBlock = true;
    }
    private void Unblock()
    {
        block = false;
        aimBlock = false;
    }

    protected void Start()
    {
        inventory = player.GetComponent<InventoryComponent>();
        inventory.InventorySystem.OnInventorySlotsChanged += OnInventorySlotUpdate;
        UpdateInventoryAmmo();
    }

    private void OnEnable()
    {
        Controls.OnRightPressed += Aim;
        Controls.OnRightReleased += StopAim;
        Controls.OnLeftReleased += StopShooting;
        Controls.OnSprintPressed += Sprint;
        Controls.OnSprintReleased += StopSprint;
        Controls.OnLeftPressed += Fire;
        Controls.OnReloadPressed += Reload;

        player.events.OnWeaponEquipped += OnWeaponEquipped;

        UIManager.OnUIClose += Unblock;
        UIManager.OnUIOpen += Block;
    }

    private void OnDisable()
    {
        Controls.OnRightPressed -= Aim;
        Controls.OnRightReleased -= StopAim;
        Controls.OnLeftReleased -= StopShooting;
        Controls.OnSprintPressed -= Sprint;
        Controls.OnSprintReleased -= StopSprint;
        Controls.OnLeftPressed -= Fire;
        Controls.OnReloadPressed -= Reload;

        player.events.OnWeaponEquipped -= OnWeaponEquipped;

        UIManager.OnUIClose -= Unblock;
        UIManager.OnUIOpen -= Block;
    }
    void Update()
    {
        if (isSprinting && !block)
            animator.SetBool("Sprint", Controls.GetMovement() != Vector2.zero);
    }

    private void Fire()
    {
        if (weapon && Cursor.lockState != CursorLockMode.Confined)
            weapon.Fire();
    }

    private void Reload()
    {
        if (weapon && Cursor.lockState != CursorLockMode.Confined
            && inventory != null && inventory.InventorySystem.ContainsItem(weapon.weapon.ammoType, out _, out _))
        {
            weapon.Reload();
        }
    }

    private void OnWeaponEquipped(GameObject weapon)
    {
        if (this.weapon != null && this.weapon.gameObject == weapon)
            return;

        if (this.weapon != null)
        {
            this.weapon.gameObject.SetActive(false);
            this.weapon.animator = null;
            this.weapon.OnFinishReload -= OnReloadFinish;
            this.weapon.OnClipAmmoChanged -= OnClipAmmoChangedImpl;
            player.events.OnWeaponUnequipped?.Invoke(this.weapon.gameObject);
        }

        var abstractWeapon = weapon.GetComponent<AbstractWeaponController>();
        abstractWeapon.gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        this.weapon = abstractWeapon;
        animator.runtimeAnimatorController = abstractWeapon.weapon.animatorController;
        abstractWeapon.animator = animator;
        weapon.SetActive(true);
        callback.proxy = abstractWeapon;
        animator.Update(0f); // Gets rid of bad animation on weapon equip

        abstractWeapon.OnFinishReload += OnReloadFinish;
        abstractWeapon.OnClipAmmoChanged += OnClipAmmoChangedImpl;

        OnClipAmmoChanged?.Invoke(abstractWeapon.AmmoInClip);
        UpdateInventoryAmmo();
    }

    private void OnClipAmmoChangedImpl(int obj)
    {
        OnClipAmmoChanged?.Invoke(obj);
    }

    private void OnReloadFinish()
    {
        int toRemove = weapon.weapon.clipAmmo - weapon.AmmoInClip;
        toRemove = inventory.InventorySystem.RemoveItem(weapon.weapon.ammoType, toRemove);

        weapon.AmmoInClip += toRemove;
        UpdateInventoryAmmo();
    }

    private void OnInventorySlotUpdate(int slot)
    {
        if(weapon == null)
            return;

        var slotItem = inventory.InventorySystem.SlotAt(slot);
        if (slotItem == null)
            return;

        if (slotItem.Item == weapon.weapon.ammoType)
        {
            UpdateInventoryAmmo();
        }
    }

    private void UpdateInventoryAmmo()
    {
        if (inventory != null && weapon)
            OnInventoryAmmoChanged?.Invoke(inventory.InventorySystem.GetItemCount(weapon.weapon.ammoType));
    }

    private void StopShooting()
    {
        weapon.StopFire();
    }

    private void StopSprint()
    {
        isSprinting = false;
        animator.SetBool("Sprint", false);
    }

    private void Sprint()
    {
        isSprinting = true;
    }

    // Only for now
    private void StopAim()
    {
        if (weapon != null)
            weapon.StopAim();
    }

    private void Aim()
    {
        if (aimBlock || weapon == null)
            return;

        weapon.Aim();
        _ = AimBlock();
    }

    async UniTaskVoid AimBlock()
    {
        aimBlock = true;
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        aimBlock = false;
    }
}
