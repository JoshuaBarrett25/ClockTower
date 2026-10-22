using System;
using UnityEngine;

public class Player : Entity
{
    public D_Player playerData;
    private PlayerController _playercontroller;
    
    public SpriteRenderer sprite;

    public override void Awake()
    {
        base.Awake();
        playerData = Instantiate(playerData);
        
        _playercontroller = GetComponent<PlayerController>();
    }

    public override void Start()
    {
        
    }

    public void Respawn()
    {        
        _playercontroller.TempSuspendActions(0.15f);
        this.gameObject.transform.position = SpawnManager.instance.GetSpawnPoint().position;
        _playercontroller.ResetValues();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name + " Outside search");
        if (LayerMask.LayerToName(other.gameObject.layer) == playerData.hazardLayer.ToString())
        {        
            Debug.Log(other.name);
            Debug.Log("Hit hazard");
            Respawn();
        }
    }
    
    public void MoveToLocation(Vector3 location)
    {
        this.transform.position = location;
    }
}
