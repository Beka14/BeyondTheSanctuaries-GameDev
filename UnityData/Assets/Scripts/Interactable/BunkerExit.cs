public class BunkerExit : Interactable
{
    public override string GetInteractText()
    {
        return "Exit Bunker";
    }

    public override void Interact(PlayerSubsystem player)
    {
        UIManager.GetUI<MapPickerUI>()?.Show();
    }

    public override void StopInteract(PlayerSubsystem player)
    {
        // Do nothing
    }
}