using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteStateManager : MonoBehaviour
{
    public RoomManager roomManager;
    public SpriteRenderer spriteRenderer;
    public Sprite normalSprite;
    public Sprite spectralSprite;

    public RuntimeAnimatorController normalAnimation;
    public RuntimeAnimatorController spectralAnimation;

    public Animator animator;


    private void Awake()
    {
        
        roomManager = GetComponentInParent<RoomManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        
        if (roomManager.roomType == RoomManager.RoomType.NormalRoom)
        {
            if (TryGetComponent(out animator))
            {
                animator.runtimeAnimatorController = normalAnimation;
            }
            else
            {
                this.spriteRenderer.sprite = normalSprite;
            }
            this.spriteRenderer.color = new Color(1, 1, 1, 1f);
        }
        else if(roomManager.roomType == RoomManager.RoomType.SpectralRoom)
        {
            if(TryGetComponent(out animator))
            {
                animator.runtimeAnimatorController = spectralAnimation;

            }
            else
            {
                this.spriteRenderer.sprite = spectralSprite;

            }
            this.spriteRenderer.color = new Color(1, 1, 1, 0.25f);
        }
    }

}
