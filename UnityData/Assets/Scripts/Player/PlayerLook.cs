using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform orientation;

    float xRotation = 0f;
    float yRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Confined)
            return;

        Vector2 mouseDelta = mouseSensitivity * 0.01f * Mouse.current.delta.ReadValue();

        yRotation += mouseDelta.x;
        xRotation -= mouseDelta.y;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
