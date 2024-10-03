using System;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class PlayerHealth : PlayerSubsystem
{
    private HealthComponent healthComponent;
    [SerializeField] private GameObject globalPlayer;

    bool godMode = false;

    private void Start()
    {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.OnDeath += Die;

        player.events.OnHeal += Heal;

        var controllerCollider = GetComponentInChildren<ControllerCollider>();
        if(controllerCollider)
        {
            controllerCollider.OnBodyHit += OnBodyHit;
        }
    }

    private void Heal(float obj)
    {
        healthComponent.CurrentHealth += obj;
    }

    private void OnBodyHit()
    {
        if (godMode)
            return;
        healthComponent.CurrentHealth -= 20;
    }

    private void Die()
    {
        globalPlayer.SetActive(false);
    }

    public bool ToggleGodMode()
    {
        return godMode = !godMode;
    }
}
