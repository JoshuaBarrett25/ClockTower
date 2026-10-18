using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CapsuleCollider2D))]
public class InteractHandler : MonoBehaviour
{
    private Player _player;
    
    public List<GameObject> interactablesInRange;
    public GameObject currentInteractable;
    public float delayBetweenInteracts = 1f;
    public bool interactLocked;

    private void Start()
    {
        _player = GetComponent<Player>();
    }
    
    
    public void InteractOption(InputAction.CallbackContext context)
    {
        if (context.performed && currentInteractable != null && !interactLocked)
        {
            Debug.Log("Trying interacting with " + currentInteractable.name);
            currentInteractable.GetComponent<Interactable>().TryInteract(_player);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            if (!interactablesInRange.Contains(other.gameObject))
            {
                interactablesInRange.Add(other.gameObject);   
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            if (interactablesInRange.Count > 0)
            {
                interactablesInRange = interactablesInRange.OrderBy(x => Vector2.Distance(this.transform.position,x.transform.position)
                ).ToList();
                currentInteractable = interactablesInRange.First();         
                HUDManager.instance.ToggleInteractText(true);
                HUDManager.instance.interactText.text = currentInteractable.GetComponent<Interactable>().GetInteractableText();
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            interactablesInRange.Remove(currentInteractable);
            if (interactablesInRange.Count <= 0)
            {
                currentInteractable = null;
                HUDManager.instance.ToggleInteractText(false);
            }
        }
    }
    
    public IEnumerator InteractCooldown()
    {
        interactLocked = true;
        yield return new WaitForSeconds(delayBetweenInteracts);
        interactLocked = false;
    }
}
