using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action OnDeath;
    public event Action<float> OnHealthChange;

    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Math.Clamp(value, 0f, maxHealth);
            OnHealthChange?.Invoke(currentHealth / maxHealth);
            if (currentHealth <= 0)
                OnDeath?.Invoke();
        }
    }
    public bool IsFull => currentHealth == maxHealth;

    private void Start()
    {
        RestoreFullHealth();
    }

    public void RestoreFullHealth()
    {
        CurrentHealth = maxHealth;
    }
}
