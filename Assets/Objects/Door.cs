using System;
using System.Collections;
using UnityEngine;

public class Door : Receiver
{
    public float doorOpenTime = 5f;
    
    public Coroutine openDoor;

    public override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        
    }

    public override void ReceiveMessage(GameObject other)
    {
        base.ReceiveMessage(other);
        if (openDoor != null)
        {
            StopCoroutine(openDoor);
        }
        openDoor = StartCoroutine(OpenDoor());
    }

    private IEnumerator OpenDoor()
    {
        Debug.Log("Door is open...");
        yield return new WaitForSeconds(doorOpenTime);
        Debug.Log("Door is closed!");
    }
}
