using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IReceiver
{
    public float doorOpenTime = 5f;
    
    private bool _isDoorOpen = false;

    public void Awake()
    {
        StartCoroutine(OpenDoor());
    }

    public void ReceiveMessage(GameObject other)
    {
        _isDoorOpen = true;
    }

    private IEnumerator OpenDoor()
    {
        while (_isDoorOpen)
        {
            Debug.Log("Door is open...");
            yield return new WaitForSeconds(doorOpenTime);
            Debug.Log("Door is closed!");
            _isDoorOpen = false;
        }
    }
}
