using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject virtualCamera;

    public enum RoomType { SpectralRoom, NormalRoom };
    public RoomType roomType;
    public GameObject spectralRoom;
    public GameObject normalRoom;
    public PlayerController playerController;

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
            normalRoom.SetActive(true);
            spectralRoom.SetActive(false);
        }
        if (roomType == RoomType.SpectralRoom)
        {
            normalRoom.SetActive(false);
            spectralRoom.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&& !collision.isTrigger)
        {
            virtualCamera.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCamera.SetActive(false);
        }
    }
}
