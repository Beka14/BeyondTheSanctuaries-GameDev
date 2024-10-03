public class RadioStation : Interactable
{
    public override string GetInteractText()
    {
        return "Escape!";
    }

    public override void Interact(PlayerSubsystem player)
    {
        UIManager.GetUI<WinUI>()?.Show();
    }

    public override void StopInteract(PlayerSubsystem player)
    {
        
    }
}