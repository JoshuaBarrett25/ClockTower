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
}
