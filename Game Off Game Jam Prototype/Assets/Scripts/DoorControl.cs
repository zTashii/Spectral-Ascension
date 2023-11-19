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
    public PlayerController playerController;
    public KeyControl keyControl;

    public List<Key> keys;
   
    public bool normalFilled;
    public bool spectralFilled;

    private void Update()
    {
        if (player)
        {
            playerController = player.GetComponent<PlayerController>();
            keyControl = player.GetComponent<KeyControl>();
            MoveKeys();

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
        int count =0;
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
        yield return new WaitForSeconds(1.2f);
        obj.SetActive(false);
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
