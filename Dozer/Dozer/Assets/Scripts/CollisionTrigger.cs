using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
 private void OnCollisionEnter(Collision other)
 {
  if (other.collider.gameObject.CompareTag("Roller"))
  {
   var interactable = GetComponent<IInteractable>();
   var dozer = other.gameObject.GetComponent<IInteractable>();
   dozer.Interact(other.collider);
   interactable.Interact(other.collider);
  }
 }
}
