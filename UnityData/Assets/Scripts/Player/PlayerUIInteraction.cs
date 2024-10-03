public class PlayerUIInteraction : PlayerSubsystem
{
    private void Start()
    {
        UIManager.BindToPlayer(this);
    }
}