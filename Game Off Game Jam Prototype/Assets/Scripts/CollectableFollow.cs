using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableFollow : MonoBehaviour
{
    public Transform player;
    public Vector2 offset;
    [Range(0, 1f)]
    public float dampen;
    Vector2 velocity;
    Vector2 dampPosition;

    public float hoverSpeed;
    public float hoverStrength;
    private Key key;

    public Vector2 randomOffset;
    public float randomOffsetRange;

    private void Awake()
    {
        key = GetComponent<Key>();
        RandomOffset();
    }


    void Follow()
    {
        Vector2 behind = new Vector2(offset.x * player.transform.localScale.x, offset.y) ;
        dampPosition = Vector2.SmoothDamp(transform.position, (Vector2)player.transform.position + behind + randomOffset, ref velocity, dampen);
        transform.position = dampPosition + BounceOffset();
    }

    private void Update()
    {
        if (key.isFollowing)
        {
            Follow();
        }
    }
    public Vector2 BounceOffset()
    {
        return new Vector2(0, Mathf.Sin(Time.time * hoverSpeed) * hoverStrength);
    }

    public Vector2 RandomOffset()
    {
        float x = Random.Range(-randomOffsetRange, randomOffsetRange);
        float y = Random.Range(-randomOffsetRange, randomOffsetRange);
        randomOffset = new Vector2(x, y);
        return randomOffset;
    }
}
