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
    public bool opened;

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
            
            

            if (!opened)
            {
                TransferKeys();
                //KeyHandler();
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
        }
        if(normalFilled && spectralFilled)
        {
            opened = true;
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
            obj.transform.position = Vector2.MoveTowards(obj.transform.position, transform.position, 6f * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator MoreWait()
    {
        yield return new WaitForSeconds(2f);
    }
    public void TransferKeys()
    {
        for (int i = 0; i < keyControl.key.Count; i++)
        {
            if(keyControl.key[i].keyType == Key.KeyType.Normal && roomManager.roomType == RoomManager.RoomType.NormalRoom || keyControl.key[i].keyType == Key.KeyType.Spectral && roomManager.roomType == RoomManager.RoomType.SpectralRoom)
            {
                keys.Add(keyControl.key[i]);
                keyControl.key[i].gameObject.transform.SetParent(this.transform);
                StartCoroutine(MoreWait());
                keyControl.key.Remove(keyControl.key[i]);
                
            }
        }
        StartCoroutine(MoreWait());
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].gameObject.activeInHierarchy)
            {
                keys[i].GetComponent<Key>().isFollowing = false;
                keys[i].GetComponent<Key>().deposited = true;
            }
            //keys[i].transform.position = Vector2.Lerp(keys[i].transform.position, transform.position, Time.deltaTime * 5);
            //keys[i].transform.position = Vector2.MoveTowards(keys[i].transform.position, transform.position, 6 * Time.deltaTime);
            //StartCoroutine(Move(keys[i].gameObject));
            //StartCoroutine(Wait(keys[i].gameObject));
        }
        
    }
    public void KeyHandler()
    {
        foreach (Key key in keyControl.key)
        {
            if (key.keyType == Key.KeyType.Normal && roomManager.roomType == RoomManager.RoomType.NormalRoom || key.keyType == Key.KeyType.Spectral && roomManager.roomType == RoomManager.RoomType.SpectralRoom)
            {
                keys.Add(key);

                key.gameObject.transform.SetParent(this.transform);
                keyControl.key.Remove(key);
            }
        }
        StartCoroutine(MoreWait());
        foreach (Key key in keys)
        {
            key.GetComponent<Key>().isFollowing = false;
            key.GetComponent<Key>().deposited = true;
            key.transform.position = Vector2.MoveTowards(key.transform.position, transform.position, 6 * Time.deltaTime);
            StartCoroutine(Move(key.gameObject));
            StartCoroutine(Wait(key.gameObject));
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
