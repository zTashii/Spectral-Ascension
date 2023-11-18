using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalController : MonoBehaviour
{
    public int requiredNormalKeys;
    public int requiredSpectralKeys;

    public Sprite normalBase;
    public Sprite spectralBase;

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
    }

    private void Update()
    {
        CheckRoomState();
        UpdatePedestal();
        //Initialize();
    }
    private void Start()
    {
        //Initialize();
        
    }


    void CheckRoomState()
    {
        if(roomManager.roomType == RoomManager.RoomType.NormalRoom)
        {
            spriteRenderer.sprite = normalBase;
        }
        else if (roomManager.roomType == RoomManager.RoomType.SpectralRoom)
        {
            spriteRenderer.sprite = spectralBase;
        }
    }

    void Initialize()
    {
        if(roomManager.roomType == RoomManager.RoomType.NormalRoom)
        {
            for(int i = 0; i < requiredNormalKeys; i++)
            {
                for (int y = 0; y < requiredSpectralKeys; y++)
                {
                    keyDisplay[i].SetActive(true);
                    tinyKeyDisplay[y].SetActive(true);
                }
            }
        }
        else if (roomManager.roomType == RoomManager.RoomType.SpectralRoom)
        {
            for (int i = 0; i < requiredSpectralKeys; i++)
            {
                for (int y = 0; y < requiredNormalKeys; y++)
                {
                    tinyKeyDisplay[y].SetActive(true);
                    keyDisplay[i].SetActive(true);
                }
            }
            
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
                    if (i < requiredNormalKeys)
                    {
                        keyDisplay[i].SetActive(true);
                    }
                    else
                    {
                        keyDisplay[i].SetActive(false);
                    }
                    if (y < requiredSpectralKeys)
                    {
                        tinyKeyDisplay[y].SetActive(true);
                    }
                    else
                    {
                        tinyKeyDisplay[y].SetActive(false);
                    }
                    


                }
                if (roomManager.roomType == RoomManager.RoomType.SpectralRoom)
                {
                    if (i < requiredSpectralKeys)
                    {
                        keyDisplay[i].SetActive(true);
                    }
                    else
                    {
                        keyDisplay[i].SetActive(false);
                    }
                    if (y < requiredNormalKeys)
                    {
                        tinyKeyDisplay[y].SetActive(true);
                    }
                    else
                    {
                        tinyKeyDisplay[y].SetActive(false);
                    }
                }
            }
        }
        

    }
}
