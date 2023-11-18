using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public GameObject door;
    public GameObject player;
    public PlayerController playerController;
    public KeyControl keyControl;

    public List<Key> keys;
    public int requiredNormalKeys;
    public int requiredSpectralKeys;
    public bool normalFilled;
    public bool spectralFilled;

    private void Update()
    {
        if (player)
        {
            playerController = player.GetComponent<PlayerController>();
            keyControl = player.GetComponent<KeyControl>();
            MoveKeys();

            if (KeyCount(Key.KeyType.Normal, keys) == requiredNormalKeys)
            {
                normalFilled = true;
            }
            else
            {
                normalFilled = false;
            }
            if (KeyCount(Key.KeyType.Spectral, keys) == requiredSpectralKeys)
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


    public int KeyCount(Key.KeyType key, List<Key> keyList)
    {
        int count =0;
        for (int i = 0; i < keyList.Count; i++)
        {

            if(keyList[i].keyType == key)
            {
                count++;
            }
        }
        return count;
    }

    //add set locations to a list
    //compare list to location list, keys lerp/movetowards locations based on location in list, if key in slot 1 in list, make that go to slot 1 in location list.
    public void MoveKeys()
    {
        for (int i = 0; i < keyControl.key.Count; i++)
        {
            keys.Add(keyControl.key[i]);
            keyControl.key[i].gameObject.transform.SetParent(this.transform);
            
            //keyControl.key[i].gameObject.transform.position = this.transform.position; //make set to a specific location;
            keyControl.key.Remove(keyControl.key[i]);

            
        }
        for (int i = 0; i < keys.Count; i++)
        {
            //for (int y = 0; y < keyLocations.Count; y++)
            //{
            //    if (i == y)
            //    {
            //        if (keys[i].keyType == Key.KeyType.Normal)
            //        {
            //            keys[i].transform.position = Vector2.Lerp(keys[i].transform.position, keyLocations[y].transform.position, Time.deltaTime * 5);
            //            keys[i].transform.position = keyLocations[y].transform.position;
            //        }
            //        if (keys[i].keyType == Key.KeyType.Spectral)
            //        {
            //            keys[i].transform.position = Vector2.Lerp(keys[i].transform.position, keyLocations[y].transform.position, Time.deltaTime * 5);

            //        }
            //    }
            //}
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
            
            
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
