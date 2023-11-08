using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectreInteractable : InteractableBase
{
    private PlayerController playerController;
    private void Start()
    {
        this.playerController = PlayerController.instance;
    }
    public override void Interact()
    {
        Debug.Log("interacted");
        this.playerController.playerState.isGhost = !this.playerController.playerState.isGhost;
        this.playerController.MoveToAnchors();
    }

    
}
