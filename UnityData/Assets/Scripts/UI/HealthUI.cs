using UnityEngine;
using UnityEngine.UI;

public class HealthUI : UISubsystem
{
    [Header("Inspector")]
    [SerializeField] private Slider healthBar;

    public override void Bind(PlayerSubsystem playerSubsystem)
    {
        var health = playerSubsystem.GetComponent<HealthComponent>();
        if (!health)
            return;
        health.OnHealthChange += OnHealthChange;
    }

    private void OnHealthChange(float obj)
    {
        healthBar.value = obj;
        gameObject.SetActive(obj > 0);
    }
}