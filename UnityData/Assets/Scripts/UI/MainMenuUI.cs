using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnExit()
    {
        GameManager.QuitGame();
    }

    public void OnPlay()
    {
        GameManager.instance.LoadLevel(1);
    }
}