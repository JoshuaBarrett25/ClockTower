using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Interactable : MonoBehaviour
{
    public D_Interactable interactableData;
    
    private CircleCollider2D _interactCollider;

    private void Awake()
    {
        _interactCollider = GetComponent<CircleCollider2D>();
    }
    

    public virtual void OnEnable()
    {
        interactableData = Instantiate(interactableData);
    }

    public virtual void OnDisable()
    {
        
    }

    public virtual void PayCost(Player player)
    {
        
    }

    public virtual string GetInteractableText()
    {
        return "Interact";
    }

    protected virtual void ExecuteInteract(Player player)
    {
        Debug.Log("Successful interact with..." + this.name);
    }
    
    public virtual void TryInteract(Player player)
    {
        Debug.Log("Trying to interact with..." + this.name);
        ExecuteInteract(player);
    }

    
}
