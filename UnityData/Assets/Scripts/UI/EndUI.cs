using UnityEngine;

public class EndUI : UISubsystem
{
    override public void Bind(PlayerSubsystem playerSubsystem)
    {
        var end = playerSubsystem.GetComponent<HealthComponent>();
        if (!end)
            return;

        end.OnDeath += Show;
    }

    override public void Close()
    {
        base.Close();
        Time.timeScale = 1;
        GameManager.ToMainMenu();
    }

    override public void Show()
    {
        if (UIManager.CurrentUI != null)
            UIManager.CurrentUI.Close();

        base.Show();
        Time.timeScale = 0;
    }

    public void OnMainMenu()
    {
        Close();
    }
}