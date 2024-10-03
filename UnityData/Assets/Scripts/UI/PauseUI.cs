using UnityEngine;

public class PauseUI : UISubsystem
{
    override public void Close()
    {
        base.Close();
        Time.timeScale = 1;
    }

    override public void Show()
    {
        base.Show();
        Time.timeScale = 0;
    }

    public void OnContinue()
    {
        Close();
    }

    public void OnMainMenu()
    {
        Close();
        GameManager.ToMainMenu();
    }
}