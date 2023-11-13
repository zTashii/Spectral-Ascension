using System;
using System.Reflection;
using UnityEngine;

[Serializable]
public class PlayerStates
{
    public bool facingRight;
    public bool onGround;
    public bool onWall;
    public bool isGhost;
    public bool canMove;
    public bool canJump;
    public bool canDash;
    public bool jumping;
    public bool isDashing;
    public bool hasDashed;
    public bool wallClimbing;
    public bool wallJumping;
    public bool wallSliding;


    public PlayerStates()
    {
        this.facingRight = false;
        this.Reset();
    }

    public bool GetState(string stateName)
    {
        FieldInfo field = base.GetType().GetField(stateName);
        if (field != null)
        {
            return (bool)field.GetValue(PlayerController.instance.playerState);
        }
        Debug.LogError("PlayerControllerStates: Couldnt find " + stateName);
        return false;
    }

    public void SetState(string stateName, bool value)
    {
        FieldInfo field = base.GetType().GetField(stateName);
        if (field != null)
        {
            try
            {
                field.SetValue(PlayerController.instance.playerState, value);
            }
            catch (Exception arg)
            {
                Debug.LogError("Failed to set playerState: " + arg);
            }
        }
        else
        {
            Debug.LogError("PlayerStates: Couldnt find " + stateName);
        }
    }

    public void Reset()
    {
        this.facingRight = false;
        this.onGround = false;
        this.onWall = false;
        this.isGhost = false;
        this.canMove = false;
        this.canJump = false;
        this.canDash = false;
        this.jumping = false;
        this.isDashing = false;
        this.hasDashed = false;
        this.wallClimbing = false;
        this.wallJumping = false;
        this.wallSliding = false;
    }
}
