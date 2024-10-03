using Cysharp.Threading.Tasks;
using UnityEngine;

public class BunkerEntrance : Interactable
{ 
    public override string GetInteractText()
    {
        return "Enter Bunker";
    }

    public override void Interact(PlayerSubsystem player)
    {
        GameManager.instance.LoadLevel(1, player);
    }

    public override void StopInteract(PlayerSubsystem player)
    {
        // Do nothing
    }
}