using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : Interactable
{
    public List<Receiver> receivers;
    
    protected override void ExecuteInteract(Player player)
    {
        receivers[0].ReceiveMessage(player.gameObject);
    }
}