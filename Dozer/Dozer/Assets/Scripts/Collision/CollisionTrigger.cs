using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
 private void OnCollisionEnter(Collision other)
 {
  if (other.collider.gameObject.CompareTag(GameController.GameConfig.DozerTag) && GameController.Status == GameStatus.Playing)
  {
   var interactable = GetComponent<IInteractable>();
   var playerController = other.gameObject.GetComponent<PlayerController>();
   interactable.Interact(playerController);
  }
 }
}
