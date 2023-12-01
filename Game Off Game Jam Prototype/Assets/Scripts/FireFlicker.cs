using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireFlicker : MonoBehaviour
{
    public Light2D fireLight;

    public float minValue;
    public float maxValue;
    public float falloffStrength;
    [Range(0f,1f)]
    public float interpolationPoint;
    float timeElapsed;
    float lerpDur= 2f;

    private RoomManager roomManager;
    public Color normalColour;
    public Color spectralColour;
    private void Awake()
    {
        roomManager = GetComponentInParent<RoomManager>();
        fireLight = GetComponent<Light2D>();
    }

    private void Update()
    {
        if(roomManager.roomType == RoomManager.RoomType.NormalRoom)
        {
            fireLight.color = normalColour;
        }
        else if (roomManager.roomType == RoomManager.RoomType.SpectralRoom)
        {
            fireLight.color = spectralColour;
        }
    }

    private void FixedUpdate()
    {
        //interpolationPoint = Random.Range(0, 1);
        //falloffStrength = Mathf.Lerp(0.4f, 0.5f, interpolationPoint);
        //falloffStrength = Random.Range(0.4f, 0.5f);
        //StartCoroutine(Mathlerp());
        StartCoroutine(Lerp());
        
        
    }

    //IEnumerator Mathlerp()
    //{
    //    interpolationPoint = Random.Range(0f, 1f);
    //    yield return new WaitForSeconds(1f);
    //    falloffStrength = Mathf.Lerp(0.4f, 0.5f, interpolationPoint);
    //}

    IEnumerator Lerp()
    {
        timeElapsed = 0;
        while (timeElapsed < lerpDur)
        {
            falloffStrength = Mathf.Lerp(minValue, maxValue, timeElapsed / lerpDur);
            timeElapsed += Time.deltaTime;
            fireLight.falloffIntensity = falloffStrength;
            yield return null;
        }
        
        falloffStrength = 0.5f;

        
    }
}
