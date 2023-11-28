using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverIcon : MonoBehaviour
{
    [SerializeField] private float hoverSpeed = 5.0f;
    [SerializeField] private float hoverStrength = 0.15f;

    public bool xAxis;
    //private float initialPosition;
    private Vector2 initialPosition;

    private Vector2 newPosition;

    private void Start()
    {
            initialPosition = transform.position;
        
    }

    private void Update()
    {
        float hoverOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverStrength;
        if (xAxis)
        {

            newPosition = new Vector3(initialPosition.x, transform.position.y) + Vector3.right * hoverOffset;

        }
        else
        {
            newPosition = new Vector3(transform.position.x, initialPosition.y) + Vector3.up * hoverOffset;

        }
        transform.position = newPosition;
    }
}
