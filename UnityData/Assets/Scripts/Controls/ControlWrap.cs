using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlWrap
{
    private readonly InputActions inputs = new();

    public event Action<InputAction.CallbackContext> OnJump
    {
        add => inputs.Player.Jump.performed += value;
        remove => inputs.Player.Jump.performed -= value;
    }


    public event Action<int> OnNumberReleased;
    public event Action<int> OnNumberPressed;
    public event Action OnUsePressed;
    public event Action OnUseReleased;

    public event Action OnThrowPressed;
    public event Action OnDuckPressed;
    public event Action OnDuckReleased;
    public event Action OnRightPressed;
    public event Action OnRightReleased;
    public event Action OnLeftPressed;
    public event Action OnLeftReleased;
    public event Action OnEscapePressed;
    public event Action OnReloadPressed;
    public event Action OnInventoryPressed;
    public event Action OnSprintPressed;
    public event Action OnSprintReleased;

    public event Action OnConsolePressed;

    event Action<InputAction.CallbackContext> OnNumberReleasedStub
    {
        add
        {
            inputs.Player.Number1.canceled += value;
            inputs.Player.Number2.canceled += value;
            inputs.Player.Number3.canceled += value;
            inputs.Player.Number4.canceled += value;
            inputs.Player.Number5.canceled += value;
            inputs.Player.Number6.canceled += value;
        }
        remove
        {
            inputs.Player.Number1.canceled -= value;
            inputs.Player.Number2.canceled -= value;
            inputs.Player.Number3.canceled -= value;
            inputs.Player.Number4.canceled -= value;
            inputs.Player.Number5.canceled -= value;
            inputs.Player.Number6.canceled -= value;
        }
    }
    event Action<InputAction.CallbackContext> OnNumberPressedStub
    {
        add
        {
            inputs.Player.Number1.performed += value;
            inputs.Player.Number2.performed += value;
            inputs.Player.Number3.performed += value;
            inputs.Player.Number4.performed += value;
            inputs.Player.Number5.performed += value;
            inputs.Player.Number6.performed += value;
        }
        remove
        {
            inputs.Player.Number1.performed -= value;
            inputs.Player.Number2.performed -= value;
            inputs.Player.Number3.performed -= value;
            inputs.Player.Number4.performed -= value;
            inputs.Player.Number5.performed -= value;
            inputs.Player.Number6.performed -= value;
        }
    }
    event Action<InputAction.CallbackContext> OnInteractPressedStub
    {
        add => inputs.Player.Interact.performed += value;
        remove => inputs.Player.Interact.performed -= value;
    }
    event Action<InputAction.CallbackContext> OnInteractReleasedStub
    {
        add => inputs.Player.Interact.canceled += value;
        remove => inputs.Player.Interact.canceled -= value;
    }
    event Action<InputAction.CallbackContext> OnThrowPressedStub
    {
        add => inputs.Player.ThrowItem.performed += value;
        remove => inputs.Player.ThrowItem.performed -= value;
    }
    event Action<InputAction.CallbackContext> OnDuckPressedStub
    {
        add => inputs.Player.Duck.performed += value;
        remove => inputs.Player.Duck.performed -= value;
    }
    event Action<InputAction.CallbackContext> OnDuckReleasedStub
    {
        add => inputs.Player.Duck.canceled += value;
        remove => inputs.Player.Duck.canceled -= value;
    }
    event Action<InputAction.CallbackContext> OnRightPressedStub
    {
        add => inputs.Player.RightClick.performed += value;
        remove => inputs.Player.RightClick.performed -= value;
    }
    event Action<InputAction.CallbackContext> OnRightReleasedStub
    {
        add => inputs.Player.RightClick.canceled += value;
        remove => inputs.Player.RightClick.canceled -= value;
    }
    event Action<InputAction.CallbackContext> OnLeftPressedStub
    {
        add => inputs.Player.LeftClick.performed += value;
        remove => inputs.Player.LeftClick.performed -= value;
    }
    event Action<InputAction.CallbackContext> OnLeftReleasedStub
    {
        add => inputs.Player.LeftClick.canceled += value;
        remove => inputs.Player.LeftClick.canceled -= value;
    }
    event Action<InputAction.CallbackContext> OnEscapePressedStub
    {
        add => inputs.Player.Escape.performed += value;
        remove => inputs.Player.Escape.performed -= value;
    }
    event Action<InputAction.CallbackContext> OnReloadPressedStub
    {
        add => inputs.Player.Reload.performed += value;
        remove => inputs.Player.Reload.performed -= value;
    }
    event Action<InputAction.CallbackContext> OnInventoryPressedStub
    {
        add => inputs.Player.Inventory.performed += value;
        remove => inputs.Player.Inventory.performed -= value;
    }
    event Action<InputAction.CallbackContext> OnSprintPressedStub
    {
        add => inputs.Player.Sprint.performed += value;
        remove => inputs.Player.Sprint.performed -= value;
    }

    event Action<InputAction.CallbackContext> OnSprintReleasedStub
    {
        add => inputs.Player.Sprint.canceled += value;
        remove => inputs.Player.Sprint.canceled -= value;
    }

    event Action<InputAction.CallbackContext> OnConsolePressedStub
    {
        add => inputs.Player.Console.performed += value;
        remove => inputs.Player.Console.performed -= value;
    }


    void OnNumberPressedProxy(InputAction.CallbackContext ctx)
    {
        if (ctx.action == inputs.Player.Number1)
        {
            OnNumberPressed?.Invoke(1);
        }
        else if (ctx.action == inputs.Player.Number2)
        {
            OnNumberPressed?.Invoke(2);
        }
        else if (ctx.action == inputs.Player.Number3)
        {
            OnNumberPressed?.Invoke(3);
        }
        else if (ctx.action == inputs.Player.Number4)
        {
            OnNumberPressed?.Invoke(4);
        }
        else if (ctx.action == inputs.Player.Number5)
        {
            OnNumberPressed?.Invoke(5);
        }
        else if (ctx.action == inputs.Player.Number6)
        {
            OnNumberPressed?.Invoke(6);
        }
    }
    void OnNumberReleasedProxy(InputAction.CallbackContext ctx)
    {
        if (ctx.action == inputs.Player.Number1)
        {
            OnNumberReleased?.Invoke(1);
        }
        else if (ctx.action == inputs.Player.Number2)
        {
            OnNumberReleased?.Invoke(2);
        }
        else if (ctx.action == inputs.Player.Number3)
        {
            OnNumberReleased?.Invoke(3);
        }
        else if (ctx.action == inputs.Player.Number4)
        {
            OnNumberReleased?.Invoke(4);
        }
        else if (ctx.action == inputs.Player.Number5)
        {
            OnNumberReleased?.Invoke(5);
        }
        else if (ctx.action == inputs.Player.Number6)
        {
            OnNumberReleased?.Invoke(6);
        }
    }
    void OnThrowPressedProxy(InputAction.CallbackContext ctx)
    {
        OnThrowPressed?.Invoke();
    }
    void OnDuckPressedProxy(InputAction.CallbackContext ctx)
    {
        OnDuckPressed?.Invoke();
    }
    void OnDuckReleasedProxy(InputAction.CallbackContext ctx)
    {
        OnDuckReleased?.Invoke();
    }
    void OnRightPressedProxy(InputAction.CallbackContext ctx)
    {
        OnRightPressed?.Invoke();
    }
    void OnRightReleasedProxy(InputAction.CallbackContext ctx)
    {
        OnRightReleased?.Invoke();
    }
    void OnLeftPressedProxy(InputAction.CallbackContext ctx)
    {
        OnLeftPressed?.Invoke();
    }
    void OnLeftReleasedProxy(InputAction.CallbackContext ctx)
    {
        OnLeftReleased?.Invoke();
    }
    void OnEscapePressedProxy(InputAction.CallbackContext ctx)
    {
        OnEscapePressed?.Invoke();
    }
    void OnReloadPressedProxy(InputAction.CallbackContext ctx)
    {
        OnReloadPressed?.Invoke();
    }
    void OnInventoryPressedProxy(InputAction.CallbackContext ctx)
    {
        OnInventoryPressed?.Invoke();
    }
    void OnSprintPressedProxy(InputAction.CallbackContext ctx)
    {
        OnSprintPressed?.Invoke();
    }
    void OnSprintReleasedProxy(InputAction.CallbackContext ctx)
    {
        OnSprintReleased?.Invoke();
    }
    void OnInteractPressedProxy(InputAction.CallbackContext ctx)
    {
        OnUsePressed?.Invoke();
    }
    void OnInteractReleasedProxy(InputAction.CallbackContext ctx)
    {
        OnUseReleased?.Invoke();
    }

    void OnConsolePressedProxy(InputAction.CallbackContext ctx)
    {
        OnConsolePressed?.Invoke();
    }
    // _____________________________

    public void Enable()
    {
        inputs.Player.Enable();
        OnNumberPressedStub += OnNumberPressedProxy;
        OnNumberReleasedStub += OnNumberReleasedProxy;
        OnInteractPressedStub += OnInteractPressedProxy;
        OnInteractReleasedStub += OnInteractReleasedProxy;
        OnThrowPressedStub += OnThrowPressedProxy;
        OnDuckPressedStub += OnDuckPressedProxy;
        OnDuckReleasedStub += OnDuckReleasedProxy;
        OnLeftPressedStub += OnLeftPressedProxy;
        OnLeftReleasedStub += OnLeftReleasedProxy;
        OnRightPressedStub += OnRightPressedProxy;
        OnRightReleasedStub += OnRightReleasedProxy;
        OnEscapePressedStub += OnEscapePressedProxy;
        OnReloadPressedStub += OnReloadPressedProxy;
        OnInventoryPressedStub += OnInventoryPressedProxy;
        OnSprintPressedStub += OnSprintPressedProxy;
        OnSprintReleasedStub += OnSprintReleasedProxy;

        OnConsolePressedStub += OnConsolePressedProxy;
    }
    public void Disable()
    {
        inputs.Player.Disable();
        OnNumberPressedStub -= OnNumberPressedProxy;
        OnNumberReleasedStub -= OnNumberReleasedProxy;
        OnInteractPressedStub -= OnInteractPressedProxy;
        OnInteractReleasedStub -= OnInteractReleasedProxy;
        OnThrowPressedStub -= OnThrowPressedProxy;
        OnDuckPressedStub -= OnDuckPressedProxy;
        OnDuckReleasedStub -= OnDuckReleasedProxy;
        OnRightPressedStub -= OnRightPressedProxy;
        OnRightReleasedStub -= OnRightReleasedProxy;
        OnLeftPressedStub -= OnLeftPressedProxy;
        OnLeftReleasedStub -= OnLeftReleasedProxy;
        OnEscapePressedStub -= OnEscapePressedProxy;
        OnReloadPressedStub -= OnReloadPressedProxy;
        OnInventoryPressedStub -= OnInventoryPressedProxy;
        OnSprintPressedStub -= OnSprintPressedProxy;
        OnSprintReleasedStub -= OnSprintReleasedProxy;

        OnConsolePressedStub -= OnConsolePressedProxy;
    }

    public Vector2 GetMovement()
    {
        return inputs.Player.Movement.ReadValue<Vector2>();
    }
}
