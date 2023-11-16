using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyControl : MonoBehaviour
{

   
    public GameObject player;
    public GameObject pickupAnchor;
    public List<Key> key; //Change to own object script to handle normal / spectral keys
    public bool followPlayer = true;
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
        if (followPlayer)
        {
            FollowPlayer();
        }
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
    public void FollowPlayer()
    {
        if (storedPositions.Count == 0)
        {
            storedPositions.Add(new Vector2(pickupAnchor.transform.position.x, pickupAnchor.transform.position.y)); //store the players currect position
            return;
        }
        else if (storedPositions[storedPositions.Count - 1] != pickupAnchor.transform.position)
        {
            //Debug.Log("Add to list");
            storedPositions.Add(new Vector2(pickupAnchor.transform.position.x, pickupAnchor.transform.position.y)); //store the position every frame
        }

        if (storedPositions.Count > followDistance)
        {
            foreach (Key keys in key) 
            { 
                keys.transform.position = Vector2.Lerp(keys.transform.position, storedPositions[0], speed * Time.deltaTime); //move
                //keys.transform.position = storedPositions[0];
                storedPositions.RemoveAt(0); //delete the position that player just move to
            }
        }

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Key"))
        {
            collision.gameObject.transform.SetParent(this.pickupAnchor.transform, true);
            collision.gameObject.transform.position = this.pickupAnchor.transform.position;
            key.Add(collision.GetComponent<Key>());
            //move to 
        }
    }

}
