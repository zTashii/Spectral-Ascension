using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    [Range(0, 4)]
    public int requiredNormalKeys;
    [Range(0, 4)]
    public int requiredSpectralKeys;

    public GameObject door;
    public GameObject player;
    public GameObject key;
    public PlayerController playerController;
    public KeyControl keyControl;

    public RoomManager roomManager;

    public List<Key> keys;
   
    public bool normalFilled;
    public bool spectralFilled;

    private void Awake()
    {
        roomManager = FindObjectOfType<RoomManager>();
    }
    private void Update()
    {
        if (player)
        {
            playerController = player.GetComponent<PlayerController>();
            keyControl = player.GetComponent<KeyControl>();
            TransferKeys();

            if (KeyCount(Key.KeyType.Normal) == requiredNormalKeys)
            {
                normalFilled = true;
            }
            else
            {
                normalFilled = false;
            }
            if (KeyCount(Key.KeyType.Spectral) == requiredSpectralKeys)
            {
                spectralFilled = true;
            }
            else
            {
                spectralFilled = false;
            }
        }
        if(normalFilled && spectralFilled)
        {
            door.SetActive(false);
        }
        
    }

    public int KeyCount(Key.KeyType key)
    {
        int count = 0;
        for (int i = 0; i < keys.Count; i++)
        {

            if(keys[i].keyType == key)
            {
                count++;
            }
        }
        return count;
    }
    IEnumerator Wait(GameObject obj)
    {
        //yield return new WaitForSeconds(1.2f);
        if (obj.transform.position == this.transform.position)
        {
            yield return new WaitForSeconds(1.1f);
            obj.SetActive(false);
        }
    }
    IEnumerator Move(GameObject obj)
    { 
        while(obj.transform.position != transform.position)
        {
            obj.transform.position = Vector2.MoveTowards(obj.transform.position, transform.position, 0.3f * Time.deltaTime);
            yield return null;
        }
    }

    public void MoveKeys()
    {
        for (int i = 0; i < keyControl.key.Count; i++)
        {
            keys.Add(keyControl.key[i]);
            keyControl.key[i].gameObject.transform.SetParent(this.transform);
            keyControl.key.Remove(keyControl.key[i]);
            
        }
        for (int i = 0; i < keys.Count; i++)
        {
            keys[i].transform.position = Vector2.Lerp(keys[i].transform.position, transform.position, Time.deltaTime * 5);
            StartCoroutine(Wait(keys[i].gameObject));
        }
    }

    public void TransferKeys()
    {
        for (int i = 0; i < keyControl.key.Count; i++)
        {
            if(keyControl.key[i].keyType == Key.KeyType.Normal && roomManager.roomType == RoomManager.RoomType.NormalRoom || keyControl.key[i].keyType == Key.KeyType.Spectral && roomManager.roomType == RoomManager.RoomType.SpectralRoom)
            {
                keys.Add(keyControl.key[i]);
                keyControl.key[i].gameObject.transform.SetParent(this.transform);
                keyControl.key.Remove(keyControl.key[i]);
            }
        }
        for (int i = 0; i < keys.Count; i++)
        {
            keys[i].GetComponent<Key>().isFollowing = false;
            keys[i].GetComponent<Key>().deposited = true;
            //keys[i].transform.position = Vector2.Lerp(keys[i].transform.position, transform.position, Time.deltaTime * 5);
            keys[i].transform.position = Vector2.MoveTowards(keys[i].transform.position, transform.position, 6 * Time.deltaTime);
            StartCoroutine(Move(keys[i].gameObject));
            StartCoroutine(Wait(keys[i].gameObject));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
            
            
        }
        if (collision.CompareTag("Key"))
        {
            Debug.Log("Collided 2");
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }
}
