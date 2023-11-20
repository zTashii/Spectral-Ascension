using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (GameManager._instance == null)
            {
                GameManager._instance = UnityEngine.Object.FindObjectOfType<GameManager>();
                if (GameManager._instance == null)
                {
                    UnityEngine.Debug.LogError("Couldnt find Gm");
                }
                else
                {
                    Object.DontDestroyOnLoad(GameManager._instance.gameObject);
                }
            }
            return GameManager._instance;
        }
    }

    private void Awake()
    {
        if (GameManager._instance == null)
        {
            GameManager._instance = this;
            UnityEngine.Object.DontDestroyOnLoad(this);

        }
        else
        {
            if (this != GameManager._instance)
            {
                UnityEngine.Object.Destroy(base.gameObject);
                return;
            }
        }
    }


}

