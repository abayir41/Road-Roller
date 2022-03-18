using UnityEngine;

/// <summary>
/// Catching collision and run under some conditions
/// </summary>
public class CollisionTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.CompareTag(GameController.GameConfig.DozerTag) &&
            GameController.Status == GameStatus.Playing)
        {
            var interactable = GetComponent<IInteractable>();
            var playerController = other.gameObject.GetComponent<PlayerController>();
            interactable.Interact(playerController);
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            Correction(other);
        }

        if (other.gameObject.CompareTag("Wall1"))
        {
            Correction(0);
        }
        else if (other.gameObject.CompareTag("Wall2"))
        {
            Correction(-90);
        }
        else if (other.gameObject.CompareTag("Wall3"))
        {
            Correction(180);
        }
        else if (other.gameObject.CompareTag("Wall4"))
        {
            Correction(90);
        }
    }

    void Correction(Collision c)
    {
        var _random = Random.Range(0, MapController.Instance.spawnPoints.Count);
        transform.position = MapController.Instance.spawnPoints[_random].transform.position;
        GetComponent<CarController>().Correction();
    }
    void Correction(int y)
    {
        GetComponent<CarController>().Correction(y);
    }
}