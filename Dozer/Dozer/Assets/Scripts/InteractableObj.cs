using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class InteractableObj : MonoBehaviour, IInteractable
{
    [SerializeField] private bool defaultDestroyable;
    [SerializeField] private float destroyThreshold;
    public void Interact(Collider collider)
    {
        if (defaultDestroyable || collider.bounds.size.magnitude > destroyThreshold)
        {
            Interaction();
        }
    }

    private void Interaction()
    {
        Destroy(gameObject);
        Debug.Log("Interacted");
    }
}
