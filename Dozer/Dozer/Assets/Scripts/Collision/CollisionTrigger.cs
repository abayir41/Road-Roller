using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
 private void OnCollisionEnter(Collision other)
 {
  if (other.collider.gameObject.CompareTag(GameController.DozerTag))
  {
   var interactable = GetComponent<IInteractable>();
   interactable.Interact();
  }
 }
}
