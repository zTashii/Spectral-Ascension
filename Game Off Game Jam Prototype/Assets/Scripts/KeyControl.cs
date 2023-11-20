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
        IgnoreCol();
    }

    void CheckPlayerAndKeyType()
    {
        foreach (Key keys in key)
        {
            if(keys.keyType == Key.KeyType.Normal && !this.playerController.playerState.isGhost)
            {
                //keys.gameObject.SetActive(true);
                keys.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            else if(keys.keyType == Key.KeyType.Spectral && this.playerController.playerState.isGhost)
            {
                //keys.gameObject.SetActive(true);
                keys.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

            }
            else
            {
                //keys.gameObject.SetActive(false);
                keys.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.4f);

            }

        }
        
    }
    void IgnoreCol()
    {

        foreach (Key key in key)
        {
            Physics2D.IgnoreCollision(key.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Key"))
        {
            collision.gameObject.transform.SetParent(null);
            //collision.gameObject.transform.position = this.pickupAnchor.transform.position;
            //collision.GetComponent<Collider2D>().enabled = false;
            key.Add(collision.GetComponent<Key>());
            //move to 
        }
        
    }

}
