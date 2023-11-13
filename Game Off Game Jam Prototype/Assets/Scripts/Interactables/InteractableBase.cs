using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    protected bool isInteractable;
    protected GameObject player;
    [SerializeField] protected GameObject interactIcon;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        interactIcon.SetActive(false);
    }

    private void Update()
    {
        if(isInteractable && UserInput.instance.controls.Player.Interact.WasPerformedThisFrame())
        {
            
            Interact();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
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

    public abstract void Interact();
}

