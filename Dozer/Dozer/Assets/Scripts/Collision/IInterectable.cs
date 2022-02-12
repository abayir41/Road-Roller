

public interface IInteractable
{
    ObjectType ObjectType { get; }
    
    int DestroyThreshold { get; }
    
    int ObjectHitPoint { get; }
    
    void Interact(PlayerController playerController);
}