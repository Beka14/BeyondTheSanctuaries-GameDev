using UnityEngine;

public class PlayerCModel : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.SetPositionAndRotation(cameraPosition.position, cameraPosition.rotation);
    }
}