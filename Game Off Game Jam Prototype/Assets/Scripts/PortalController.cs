using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public PlayerController player;
    public float suckSpeed;

    private void Update()
    {
        if (player)
        {
            Suck();
            if (player.gameObject.transform.position == this.transform.position)
            {
                //reload scene
                player.faderAnimator.SetTrigger("FadeOut");
                Destroy(player.gameObject);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                //teleport player to 0,0
            }
        }
        
    }

    void Suck()
    {
        player.transform.position = Vector2.MoveTowards(player.transform.position, this.transform.position, suckSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerController>();
            player.playerState.canMove = false;
            player.playerState.canFling = false;
            //player.spectralAnchors = null;
           

        }
    }
}
