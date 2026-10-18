using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    public TextMeshProUGUI interactText;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    public void ToggleInteractText(bool toggle)
    {
        if (interactText != null)
        {
            interactText.gameObject.SetActive(toggle);
        }
    }

    public void UpdateInteractText(string interactableName)
    {
        interactText.text = interactableName;
    }
}
