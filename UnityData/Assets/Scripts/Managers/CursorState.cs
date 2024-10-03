using UnityEngine;

public static class CursorState
{
    public static void SetVisible(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.Confined : CursorLockMode.Locked;
    }
}
