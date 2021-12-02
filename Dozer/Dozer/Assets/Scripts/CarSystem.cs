using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSystem : MonoBehaviour,IInteractable
{
    public void Interact(Collider collider)
    {
        Debug.Log("Dozer callback");
    }
}
