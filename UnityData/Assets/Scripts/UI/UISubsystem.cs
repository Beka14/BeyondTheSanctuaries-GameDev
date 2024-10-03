using System.Collections.Generic;
using UnityEngine;

public class UISubsystem : MonoBehaviour
{
    // Bind the subsystem to the player's event system
    public virtual void Bind(PlayerSubsystem playerSubsystem) { }

    public virtual void Close() 
    {
        UIManager.CurrentUI = null;
        gameObject.SetActive(false);
        CursorState.SetVisible(false);
        UIManager.OnUIClose?.Invoke();
    }

    public virtual void Show() 
    {
        UIManager.CurrentUI = this;
        gameObject.SetActive(true);
        CursorState.SetVisible(true);
        UIManager.OnUIOpen?.Invoke();
    }

    public virtual void Toggle()
    {
        if (UIManager.CurrentUI == this)
        {
            Close();
        }
        else if (UIManager.CurrentUI == null)
        {
            Show();
        }
    }
}
