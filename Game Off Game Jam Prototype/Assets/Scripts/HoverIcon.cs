using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverIcon : MonoBehaviour
{
    [SerializeField] private float hoverSpeed = 5.0f;
    [SerializeField] private float hoverStrength = 0.15f;

    private float initialYPosition;

    private void Start()
    {
        initialYPosition = transform.position.y;
    }

    private void Update()
    {
        float hoverOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverStrength;
        Vector3 newPosition = new Vector3(transform.position.x, initialYPosition) + Vector3.up * hoverOffset;
        transform.position = newPosition;
    }
}
