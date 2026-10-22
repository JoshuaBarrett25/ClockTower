using UnityEngine;

namespace Interactables
{
    public class Teleport : Interactable
    {
        public bool isOneWay = false;
        public GameObject destination;

        public void Awake()
        {
            if (destination == null)
            {
                Debug.Log("End point not set on..." + gameObject.name);
            }
        }
    

        protected override void ExecuteInteract(Player player)
        {
            base.ExecuteInteract(player);
            //QUEUE TRANSITION
        
            //MOVE PLAYER TO LOCATION ON OPPOSITE END
            player.MoveToLocation(destination.transform.position);
        
            //ADDITIONAL
        }
    }
}
