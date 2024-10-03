using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact(PlayerSubsystem player);
    public abstract void StopInteract(PlayerSubsystem player);
    public abstract string GetInteractText();
}