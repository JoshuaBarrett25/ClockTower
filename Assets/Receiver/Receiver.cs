using UnityEngine;

public interface IReceiver
{
    public void ReceiveMessage(GameObject other)
    {
        Debug.Log("Received direction from..." + other.name);
    }
}
