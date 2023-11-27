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

    private void Update()
    {
        if (deposited)
        {
            this.transform.position = Vector2.Lerp(this.transform.position, gameObject.transform.parent.position, 7f * Time.deltaTime);
            
            StartCoroutine(Wait());
            
            
        }
        
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.1f);
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            follow.player = collision.transform;
            isFollowing = true;
        }
        
    }


}
