
using UnityEngine;
using UnityEngine.Animations.Rigging;

class WeaponPoseUpdater : MonoBehaviour
{
    [SerializeField] MultiPositionConstraint multiPositionConstraint;
    public float weight = 1;

    private void Update()
    {
        multiPositionConstraint.weight = weight;
    }
}
