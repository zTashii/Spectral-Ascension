using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpriteRandomizer : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite spectralSprite;

    private float timeSinceChange = 0f;
    public float timeToChange = 0.1f;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timeSinceChange += Time.deltaTime;
       if(spriteRenderer != null && timeSinceChange >= timeToChange)
       {
            if(spriteRenderer.sprite == normalSprite)
            {
                spriteRenderer.sprite = spectralSprite;
            }
            else
            {
                spriteRenderer.sprite = normalSprite;
            }
            timeSinceChange = 0f;
       }
    }

 
}
