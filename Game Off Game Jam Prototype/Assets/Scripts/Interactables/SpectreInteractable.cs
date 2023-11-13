using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectreInteractable : InteractableBase
{
    private PlayerController playerController;
    [SerializeField]
    private RoomManager roomManager;
    private void Start()
    {
        this.roomManager = GetComponentInParent<RoomManager>();
        this.playerController = PlayerController.instance;
    }
    public override void Interact()
    {
        //if(roomManager.roomType == RoomManager.RoomType.NormalRoom)
        //{
        //    roomManager.roomType = RoomManager.RoomType.SpectralRoom;
        //}
        //if (roomManager.roomType == RoomManager.RoomType.SpectralRoom)
        //{
        //    roomManager.roomType = RoomManager.RoomType.NormalRoom;
        //}
        playerController.interactableSpectralAnchor = this.gameObject;
        this.playerController.playerState.canMove = !this.playerController.playerState.canMove;
        this.playerController.playerState.isGhost = !this.playerController.playerState.isGhost;
        this.playerController.MoveToAnchors();
    }

    
}
