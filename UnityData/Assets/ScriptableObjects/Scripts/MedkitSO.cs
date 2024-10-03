using UnityEngine;

[CreateAssetMenu(fileName = "Medkit", menuName = "Item/Medkit", order = 0)]
public class MedkitSO : UsableItemSO
{
    public int healAmount;

    public override bool Use(PlayerSubsystem playerSubsystem)
    {
        var health = playerSubsystem.GetComponent<HealthComponent>();
        if (health == null)
        {
            Debug.LogError("Player does not have a HealthComponent");
            return false;
        }

        if(health.IsFull)
            return false;

        health.CurrentHealth += healAmount;
        return true;
    }
}
