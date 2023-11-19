using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyControl : MonoBehaviour
{

   
    public GameObject player;
    public List<Key> key; 

    public PlayerController playerController;
    //Follow Target Variables;
    [SerializeField] private List<Vector3> storedPositions;
    [SerializeField] private int followDistance;

    public float speed;

    private void Awake()
    {
        player = this.gameObject;
        playerController = GetComponent<PlayerController>();

    }
    private void Update()
    {
        CheckPlayerAndKeyType();
    }

    void CheckPlayerAndKeyType()
    {
        foreach (Key keys in key)
        {
            if(keys.keyType == Key.KeyType.Normal && !this.playerController.playerState.isGhost)
            {
                keys.gameObject.SetActive(true);
            }
            else if(keys.keyType == Key.KeyType.Spectral && this.playerController.playerState.isGhost)
            {
                keys.gameObject.SetActive(true);
            }
            else
            {
                keys.gameObject.SetActive(false);
            }
        
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Key"))
        {
            //collision.gameObject.transform.SetParent(this.pickupAnchor.transform, true);
            //collision.gameObject.transform.position = this.pickupAnchor.transform.position;
            collision.GetComponent<Collider2D>().enabled = false;
            key.Add(collision.GetComponent<Key>());
            //move to 
        }
        
    }

}
