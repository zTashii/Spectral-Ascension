using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public enum KeyType { Normal, Spectral };
    public KeyType keyType;

    public bool isFollowing;
    public bool deposited;

    public CollectableFollow follow;

    private void Awake()
    {
        follow = GetComponent<CollectableFollow>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            follow.player = collision.transform;
            isFollowing = true;
        }
        if(collision.CompareTag("Key Pedestal"))
        {
            deposited = true;
            isFollowing = false;
            
        }
    }


}
