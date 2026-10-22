using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Interactable : MonoBehaviour
{
    public D_Interactable interactableData;
    
    private CircleCollider2D _interactCollider;
    private bool _isInteractOnCooldown = false;

    private void OnValidate()
    {
        if (_interactCollider == null)
        {
            _interactCollider = GetComponent<CircleCollider2D>();
            Debug.Log("Setting interact collider for: " + this.gameObject.name + " to trigger status: " + _interactCollider.isTrigger);
            _interactCollider.isTrigger = true;
        }
    }

    private void Awake()
    {
        _isInteractOnCooldown = false;
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

    private IEnumerator InteractCooldown(float duration)
    {
        _isInteractOnCooldown = true;
        while (_isInteractOnCooldown)
        {
            yield return new WaitForSeconds(duration);
            _isInteractOnCooldown = false;
        }
    }

    protected virtual void ExecuteInteract(Player player)
    {
        Debug.Log("Successful interact with..." + this.name);
        StartCoroutine(InteractCooldown(interactableData.interactCooldown));
    }
    
    public virtual void TryInteract(Player player)
    {
        Debug.Log("Trying to interact with..." + this.name);
        if (!_isInteractOnCooldown)
        {
            ExecuteInteract(player);   
        }
    }

    
}
