using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool triggered = false;

    public GameObject dialogueBox;

    public PlayerController player;

    public GameObject[] gameObjectsToActivate;

    public bool canTriggerInGhost;


    private void Awake()
    {
        player = PlayerController.instance;
    }

    private void Update()
    {
        if (triggered)
        {
            foreach(GameObject obj in gameObjectsToActivate)
            {
                obj.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!triggered)
            {
                if (canTriggerInGhost&& this.player.playerState.isGhost)
                {
                    
                    dialogueBox.SetActive(true);
                    triggered = true;
                    
                }
                else if(!this.player.playerState.isGhost)
                {
                    dialogueBox.SetActive(true);
                    triggered = true;
                }
                
                
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (dialogueBox.activeInHierarchy)
            {
                player.rBody.velocity = Vector2.zero;
                player.AffectedByGravity(false);
                
                player.inDialogue = true;
                player.playerState.canMove = false;
            }
            else
            {
                player.AffectedByGravity(true);
                player.inDialogue = false;
                player.playerState.canMove = true;
            }
            
        }
    }

}
