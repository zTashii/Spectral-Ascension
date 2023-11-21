using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectreInteractable : InteractableBase
{
    private PlayerController playerController;
    public bool locked;
    private void Start()
    {
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
        //playerController.interactableSpectralAnchor = this.gameObject;
        if (!playerController.playerState.isGhost)
        {
            playerController.animator.SetTrigger("GhostIn");
        }
        else
        {
            playerController.animator.SetTrigger("GhostOut");
        }
        this.playerController.playerState.canMove = !this.playerController.playerState.canMove;
        this.playerController.playerState.isGhost = !this.playerController.playerState.isGhost;
        this.playerController.MoveToAnchors();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == player)
        {
            playerController.interactableSpectralAnchor = this.gameObject;
            interactIcon.SetActive(true);
            isInteractable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            interactIcon.SetActive(false);
            isInteractable = false;
        }
    }


}
