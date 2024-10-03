using UnityEngine;

public class WinUI : UISubsystem
{
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
}
