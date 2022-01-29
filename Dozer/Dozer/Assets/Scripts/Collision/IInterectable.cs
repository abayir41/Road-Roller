public enum ObjectType : int
{
    Small,
    Mid,
    Big,
    Mega
}

public interface IInteractable
{
    ObjectType ObjectType { get; }
    
    int DestroyThreshold { get; }
    
    int ObjectHitPoint { get; }
    
    void Interact(PlayerController playerController);
}