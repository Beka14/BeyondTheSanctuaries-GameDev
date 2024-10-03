using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    UISubsystem currentUI = null;

    static public Action OnUIOpen;
    static public Action OnUIClose;

    public static UISubsystem CurrentUI
    {
        get => instance.currentUI;
        set => instance.currentUI = value;
    }


    [Header("UI Elements")]
    [SerializeField] PauseUI pauseUI = null;
    [SerializeField] List<UISubsystem> ui = new();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void EscapePressed()
    {
        if (currentUI != null)
        {
            currentUI.Close();
        }
        else if (pauseUI != null)
        {
            pauseUI.Toggle();
        }
    }

    public void BindToPlayerImpl(PlayerSubsystem playerSubsystem)
    {
        playerSubsystem.Controls.OnEscapePressed += EscapePressed;
        foreach (var subsystem in ui)
        {
            subsystem.Bind(playerSubsystem);
        }
    }

    public T GetUIImpl<T>() where T : UISubsystem
    {
        foreach (var subsystem in ui)
        {
            if (subsystem.GetType() == typeof(T))
            {
                return (T)subsystem;
            }
        }
        return null;
    }

    public static T GetUI<T>() where T : UISubsystem
    {
        return instance.GetUIImpl<T>();
    }
    public static void BindToPlayer(PlayerSubsystem playerSubsystem)
    {
        instance.BindToPlayerImpl(playerSubsystem);
    }

    public static void DisableUI()
    {
        if(instance)
            instance.gameObject.SetActive(false);
    }

    public static void EnableUI()
    {
        if (instance)
            instance.gameObject.SetActive(true);
    }
}
