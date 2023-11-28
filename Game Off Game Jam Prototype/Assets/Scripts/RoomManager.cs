using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    public GameObject virtualCamera;

    public enum RoomType { SpectralRoom, NormalRoom };
    public RoomType roomType;
    public GameObject spectralRoom;
    public GameObject normalRoom;
    public PlayerController playerController;
    public GameObject normalRoomElements;
    public GameObject spectralRoomElements;
    private void Start()
    {
        playerController = PlayerController.instance;
        this.roomType = RoomType.NormalRoom;
        virtualCamera = this.transform.Find("Virtual Camera").gameObject;
    }

    private void Update()
    {
        if (playerController.playerState.isGhost)
        {
            roomType = RoomType.SpectralRoom;
        }
        else
        {
            roomType = RoomType.NormalRoom;
        }
        CheckRoom();
    }
    public void CheckRoom()
    {
        if (roomType == RoomType.NormalRoom)
        {
            //normalRoom.SetActive(true);
            //spectralRoom.SetActive(false);
            normalRoom.GetComponentInChildren<Tilemap>().color = new Color(1, 1, 1, 1f);
            normalRoom.GetComponentInChildren<TilemapCollider2D>().enabled = true;
            spectralRoom.GetComponentInChildren<Tilemap>().color = new Color(1, 1, 1, 0.15f);
            spectralRoom.GetComponentInChildren<TilemapCollider2D>().enabled = false;
            normalRoomElements.SetActive(true);
            spectralRoomElements.SetActive(false);
            
        }
        if (roomType == RoomType.SpectralRoom)
        {
            //normalRoom.SetActive(false);
            //spectralRoom.SetActive(true);
            normalRoom.GetComponentInChildren<Tilemap>().color = new Color(1, 1, 1, 0.15f);
            normalRoom.GetComponentInChildren<TilemapCollider2D>().enabled = false;
            spectralRoom.GetComponentInChildren<Tilemap>().color = new Color(1, 1, 1, 1f);
            spectralRoom.GetComponentInChildren<TilemapCollider2D>().enabled = true;
            normalRoomElements.SetActive(false);
            spectralRoomElements.SetActive(true);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCamera.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCamera.SetActive(false);
        }
    }
}
