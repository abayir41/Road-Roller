using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class InteractableObj : MonoBehaviour, IInteractable
{
    [SerializeField] private bool defaultDestroyable;
    [SerializeField] private float destroyThreshold;
    [SerializeField] private GameObject particleEffect;
    [SerializeField] private Transform particlePos;
    public void Interact(Collider collider)
    {
        if (defaultDestroyable || collider.bounds.size.magnitude > destroyThreshold)
        {
            Interaction();
        }
    }

    private void Interaction()
    {
        var particle = Instantiate(particleEffect);
        particle.transform.position = particlePos.position;
        Destroy(gameObject);
        Debug.Log("Interacted");
    }
}
