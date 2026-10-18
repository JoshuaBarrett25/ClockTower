using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : Interactable
{
    public List<IReceiver> receivers;
    
    protected override void ExecuteInteract(Player player)
    {
        receivers[0].ReceiveMessage(player.gameObject);
    }
}