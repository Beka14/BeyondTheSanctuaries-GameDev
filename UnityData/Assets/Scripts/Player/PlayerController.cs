using System;
using UnityEngine;

public struct PlayerEvents
{
    public Action<float> OnHeal;
    public Action<int> OnDamage;

    public Action OnInventoryOpen;
    public Action<GameObject> OnWeaponEquipped;
    public Action<GameObject> OnWeaponUnequipped;
}


public class PlayerController : MonoBehaviour
{
    public ControlWrap controls;
    public PlayerEvents events;
    public bool initialized = false;

    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        if(!initialized)
        {
            controls = new();
            events = new();
            initialized = true;
        }
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.OnInventoryPressed += OnInventory;
    }
    private void OnDisable()
    {
        controls.Disable();
        controls.OnInventoryPressed -= OnInventory;
    }

    private void OnInventory()
    {
        events.OnInventoryOpen?.Invoke();
    }
}
