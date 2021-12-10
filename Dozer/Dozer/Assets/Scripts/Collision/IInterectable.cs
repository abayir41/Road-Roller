
using UnityEngine;

public enum ObjectType
{
    Small,
    House
}

public interface IInteractable
{
    ObjectType ObjectType { get; }
    
    int ObjectHitPoint { get; }
    
    void Interact(PlayerController playerController);
}