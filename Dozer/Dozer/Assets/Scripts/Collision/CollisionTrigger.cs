using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
 private void OnCollisionEnter(Collision other)
 {
  if (other.collider.gameObject.CompareTag(GameController.Instance.DozerTag))
  {
   var interactable = GetComponent<IInteractable>();
   var playerController = other.gameObject.GetComponent<PlayerController>();
   interactable.Interact(playerController);
  }
 }
}
