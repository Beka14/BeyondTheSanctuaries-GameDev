using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] bodies;
    private Animator animator;

    private void Awake()
    {
        bodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        DectivateRagdoll();
    }

    public void ActivateRagdoll()
    {
        animator.enabled = false;
        foreach (var rb in bodies)
        {
            rb.isKinematic = false;
        }
    }

    public void DectivateRagdoll()
    {
        foreach(var rb in bodies)
        {
            rb.isKinematic = true;
        }
        animator.enabled = true;
    }
}
