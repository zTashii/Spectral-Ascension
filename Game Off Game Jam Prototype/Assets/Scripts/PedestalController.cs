using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class PedestalController : MonoBehaviour
{
    public Sprite spriteBase;
    public Animator animator;
    public Sprite normal;
    public Sprite spectral;
    public Sprite inactive;

    public Sprite tinyNormal;
    public Sprite tinySpectral;
    public Sprite tinyInactive;

    public List<GameObject> keyDisplay;
    public List<GameObject> tinyKeyDisplay;

    [SerializeField] SpriteRenderer spriteRenderer;
    
    public RoomManager roomManager;
    public DoorControl doorControl;
    private void Awake()
    {
        doorControl = GetComponent<DoorControl>();
        roomManager = FindAnyObjectByType<RoomManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        CheckRoomState();
        UpdatePedestal();
        
    }


    void CheckRoomState()
    {
        if(roomManager.roomType == RoomManager.RoomType.NormalRoom)
        {
            spriteRenderer.sprite = spriteBase;
            animator.SetTrigger("isNormal");
        }
        else if (roomManager.roomType == RoomManager.RoomType.SpectralRoom)
        {
            spriteRenderer.sprite = spriteBase;
            animator.SetTrigger("isSpectral");
            }
    }

    void UpdatePedestal()
    {
        for (int i = 0; i < keyDisplay.Count; i++)
        {
            for (int y = 0; y < tinyKeyDisplay.Count; y++)
            {
                if (roomManager.roomType == RoomManager.RoomType.NormalRoom)
                {
                    if (i < doorControl.requiredNormalKeys)
                    {
                        keyDisplay[i].SetActive(true);
                    }
                    else
                    {
                        keyDisplay[i].SetActive(false);
                    }
                    if (y < doorControl.requiredSpectralKeys)
                    {
                        tinyKeyDisplay[y].SetActive(true);
                    }
                    else
                    {
                        tinyKeyDisplay[y].SetActive(false);
                    }
                    if(i < doorControl.KeyCount(Key.KeyType.Normal))
                    {
                        keyDisplay[i].GetComponent<SpriteRenderer>().sprite = normal;
                    }
                    else if(y < doorControl.KeyCount(Key.KeyType.Spectral))
                    {
                        tinyKeyDisplay[y].GetComponent<SpriteRenderer>().sprite = tinySpectral;
                    }
                    else
                    {
                        keyDisplay[i].GetComponent<SpriteRenderer>().sprite = inactive;
                        tinyKeyDisplay[y].GetComponent<SpriteRenderer>().sprite = tinyInactive;
                    }


                }
                if (roomManager.roomType == RoomManager.RoomType.SpectralRoom)
                {
                    if (i < doorControl.requiredSpectralKeys)
                    {
                        keyDisplay[i].SetActive(true);
                    }
                    else
                    {
                        keyDisplay[i].SetActive(false);
                    }
                    if (y < doorControl.requiredNormalKeys)
                    {
                        tinyKeyDisplay[y].SetActive(true);
                    }
                    else
                    {
                        tinyKeyDisplay[y].SetActive(false);
                    }
                    if (i < doorControl.KeyCount(Key.KeyType.Spectral))
                    {
                        keyDisplay[i].GetComponent<SpriteRenderer>().sprite = spectral;
                    }
                    else if (y < doorControl.KeyCount(Key.KeyType.Normal))
                    {
                        tinyKeyDisplay[y].GetComponent<SpriteRenderer>().sprite = tinyNormal;
                    }
                    else
                    {
                        keyDisplay[i].GetComponent<SpriteRenderer>().sprite = inactive;
                        tinyKeyDisplay[y].GetComponent<SpriteRenderer>().sprite = tinyInactive;
                    }
                }
            }
        }
        
       

    }
}
