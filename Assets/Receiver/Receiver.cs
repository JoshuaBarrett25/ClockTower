using UnityEngine;

public class Receiver : MonoBehaviour
{
    public virtual void Awake()
    {
        
    }
    
    public virtual void ReceiveMessage(GameObject other)
    {
        Debug.Log("Received direction from..." + other.name);
    }
}
