using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //bool or states

    private Vector2 movement;
    private Vector2 mouseDownPos;
    private Vector2 anchorPosition;

    public Rigidbody2D rBody;
    public float moveSpeed;
    public float jumpVelocity;
    public float slideSpeed;
    public float climbSpeed;
    public Vector2 dashVelocity;
    public int direction;
    public float wallJumpLerp;
    public Vector2 wallJumpVelocity;
    public int wallDirX;

    public float previousGravityScale;

    public AnimationCurve curve;
    //public AnimationCurve gravity;
    private GameManager gameManager;
    public float maxPull;

    public float maxSpeed;

    private float normalizedPull;
    private Vector2 ghostDirection;

    public PlayerStates playerState;
    [SerializeField]
    private Collider2D col2D;
    public LayerMask groundMask;

    private Vector2 worldMousePos;

    private static PlayerController _instance;
    [SerializeField]
    private float raycastLength;

    public Collider2D spectralAnchors;
    public GameObject interactableSpectralAnchor;

    public Animator animator;

    public static PlayerController instance
    {
        get
        {
            if (PlayerController._instance == null)
            {
                PlayerController._instance = UnityEngine.Object.FindObjectOfType<PlayerController>();
                if (PlayerController._instance == null)
                {
                    UnityEngine.Debug.LogError("Couldnt find player");
                }
                else
                {
                    Object.DontDestroyOnLoad(PlayerController._instance.gameObject);
                }
            }
            return PlayerController._instance;
        }
    }

    private void Awake()
    {
        if (PlayerController._instance == null)
        {
            PlayerController._instance = this;
            UnityEngine.Object.DontDestroyOnLoad(this);
        }
        else if (this != PlayerController._instance)
        {
            UnityEngine.Object.Destroy(base.gameObject);
            return;
        }
        this.playerState.canMove = true;
        this.playerState.isGhost = false;
        this.FaceRight();

        gameManager = GameManager.instance;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        SetupReferences();
        movement = Vector2.zero;
        this.playerState.canFling = true;
        //anchorPosition = transform.position;
    }
    private void FixedUpdate()
    {
        movement = UserInput.instance.moveInput;
        
        if (this.playerState.canMove)
        {
            animator.SetFloat("xVelocity", Mathf.Abs(this.rBody.velocity.x));
            
            //this.playerState.isGhost = false;
            Move(movement.x);
            if (this.playerState.wallGrip)
                return;
            if (this.movement.x > 0f && !this.playerState.facingRight)
            {
                this.FlipSprite();
            }
            if (this.movement.x < 0f && this.playerState.facingRight)
            {
                this.FlipSprite();
            }
        }
    }

    private void SetupReferences()
    {
        if(this.playerState == null)
        {
            this.playerState = new PlayerStates();
        }
        this.rBody = base.GetComponent<Rigidbody2D>();
        this.col2D = base.GetComponent<Collider2D>();
    }


    private void Update()
    {
        if (this.playerState.isGhost)
        {
            animator.SetBool("isGhost", this.playerState.isGhost);
            this.playerState.canMove = false;
            GetMousePos();
            movement = Vector2.zero;
            
        }
        else
        {
            animator.SetBool("isGhost", this.playerState.isGhost);
            this.rBody.bodyType = RigidbodyType2D.Dynamic;
            //this.playerState.canMove = true;
            Jump();
            Dash();
            WallSlide();
            WallClimb();
            WallJump();

            animator.SetBool("isWallSliding", this.playerState.wallSliding);
            animator.SetBool("isClimbing", this.playerState.wallClimbing);
            animator.SetBool("isGrippingWall", this.playerState.wallGrip);
            animator.SetFloat("yVelocity", movement.y);

        }
        CheckOnGround();
        CheckOnWall();
        
        
    }

    public void Move(float direction)
    {
        if (this.playerState.wallGrip)
            return;
        if (!this.playerState.wallJumping)
            this.rBody.velocity = new Vector2(direction * moveSpeed, this.rBody.velocity.y);
        else
        {

        }
    }
    public void FlipSprite()
    {
        this.playerState.facingRight = !this.playerState.facingRight;
        Vector3 localScale = this.transform.localScale;
        localScale.x *= -1f;
        direction *= -1;
        this.transform.localScale = localScale;
    }
    public void FaceRight()
    {
        this.playerState.facingRight = true;
        Vector3 localScale = this.transform.localScale;
        localScale.x *= 1f;
        direction = 1;
        this.transform.localScale = localScale;
    }
    public void FaceLeft()
    {
        this.playerState.facingRight = false;
        Vector3 localScale = this.transform.localScale;
        localScale.x *= -1f;
        direction = -1;
        this.transform.localScale = localScale;
    }
    public void Jump()
    {
        if (UserInput.instance.controls.Player.Jump.WasPressedThisFrame() && this.playerState.canJump)
        {
            if (this.playerState.onGround)
            {
                this.rBody.velocity = new Vector2(this.rBody.velocity.x, this.jumpVelocity);
            }
            //if (this.playerState.wallSliding || this.playerState.wallClimbing)
            //{
            //    this.playerState.wallJumping = true;
            //    this.playerState.wallSliding = false;
            //    this.playerState.wallClimbing = false;
            //    this.playerState.canMove = false;

            //    this.rBody.velocity = Vector2.zero;
            //    this.rBody.velocity = new Vector2(this.wallJumpVelocity.x * -direction, this.wallJumpVelocity.y);
            //    this.FlipSprite();
            //    StartCoroutine(JumpWait());

            //}
            if (this.playerState.onWall && !this.playerState.onGround)
            {
                StartCoroutine(JumpWait(0.1f));
                this.playerState.wallJumping = true;
                this.rBody.velocity = new Vector2(this.wallJumpVelocity.x * -direction, this.wallJumpVelocity.y);
                this.FlipSprite();
                
            }

        }
        if (this.rBody.velocity.y < 0)
        {
            this.rBody.velocity += new Vector2(0f, Physics2D.gravity.y * Time.deltaTime);
        }
        else if (UserInput.instance.controls.Player.Jump.WasReleasedThisFrame())
        {
            if (this.rBody.velocity.y > 0)
            {
                this.rBody.velocity = new Vector2(this.rBody.velocity.x, this.rBody.velocity.y * Physics2D.gravity.y * Time.deltaTime);
            }
        }
    }
    public void Dash()
    {
        if (UserInput.instance.controls.Player.Dash.WasPerformedThisFrame() && !this.playerState.wallSliding && this.playerState.canDash)
        {
            if ((movement.x != 0 || movement.y != 0) && !this.playerState.hasDashed)
            {
                this.playerState.hasDashed = true;
                this.playerState.canMove = false;
                this.rBody.velocity = Vector2.zero;
                this.rBody.velocity = new Vector2(this.dashVelocity.x * direction, this.dashVelocity.y);
                StartCoroutine(DashWait());
            }
        }
    }
    IEnumerator JumpWait(float time)
    {
        this.playerState.canMove = false;
        yield return new WaitForSeconds(time);
        this.playerState.canMove = true;
    }
    IEnumerator DashWait()
    {
        
        //this.rBody.gravityScale = 0;
        this.playerState.canJump = false;
        this.playerState.isDashing = true;
        this.playerState.wallJumping = true;
        yield return new WaitForSeconds(0.3f);
        this.playerState.canJump = true;
        this.playerState.wallJumping = false;
        //this.rBody.gravityScale = 7;
        this.playerState.isDashing = false;
        this.playerState.canMove = true;

    }
    public void WallSlide()
    {
        if (!this.playerState.canMove)
            return;
        if (this.playerState.onWall && !this.playerState.onGround)
        {

            if (movement.x != 0 && !this.playerState.wallGrip)
            {
                
                this.playerState.wallSliding = true;
                this.rBody.velocity = new Vector2(this.rBody.velocity.x, -slideSpeed);
            }
        }
        else if(this.playerState.onWall && this.playerState.onGround)
        {
            this.playerState.wallSliding = false;
        }
    }
    public void WallClimb()
    {
        if (this.playerState.onWall && UserInput.instance.controls.Player.Grip.ReadValue<float>() > 0.1f)
        {
            this.playerState.wallGrip = true;
            this.playerState.wallSliding = false;
     

        }
        else if (!this.playerState.onWall || UserInput.instance.controls.Player.Grip.WasReleasedThisFrame())
        {
            this.playerState.wallSliding = false;
            this.playerState.wallGrip = false;
            
        }
        
        if (this.playerState.wallGrip && !this.playerState.wallJumping)
        {
            AffectedByGravity(false);
            this.rBody.velocity = new(this.rBody.velocity.x, 0f);
        }
        else
        {
            AffectedByGravity(true);
        }
        if (this.playerState.wallGrip && this.movement.y != 0f)
        {
            this.playerState.wallClimbing = true;
            this.rBody.velocity = new Vector2(0f, movement.y * climbSpeed);

        }
    }
    public void AffectedByGravity(bool value)
    {
        float gravityScale = this.rBody.gravityScale;
        if (this.rBody.gravityScale > Mathf.Epsilon && !value)
        {
            this.previousGravityScale = this.rBody.gravityScale;
            this.rBody.gravityScale = 0f;
        }
        else if (this.rBody.gravityScale <= Mathf.Epsilon && value)
        {
            this.rBody.gravityScale = this.previousGravityScale;
            this.previousGravityScale = 0f;
        }
    }
    public void WallJump()
    {
        if (this.playerState.wallJumping)
        {
            this.rBody.velocity = Vector2.Lerp(this.rBody.velocity, (new Vector2(movement.x * moveSpeed, this.rBody.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    private void CheckOnGround()
    {
        if (this.CheckForGround())
        {
            this.playerState.onGround = true;
            this.playerState.hasDashed = false;
            //this.playerState.isGhost = false;
            this.playerState.wallJumping = false;
            this.playerState.canDash = false;
            this.playerState.canJump = true;
            this.playerState.canMove = true;
        }
        else
        {
            this.playerState.onGround = false;
            this.playerState.canDash = true;
        }
    }
    public bool CheckForGround()
    {
        Vector2 vector = new Vector2(this.col2D.bounds.min.x, this.col2D.bounds.center.y);
        Vector2 vector2 = this.col2D.bounds.center;
        Vector2 vector3 = new Vector2(this.col2D.bounds.max.x, this.col2D.bounds.center.y);
        float distance = this.col2D.bounds.extents.y + 0.16f;
        UnityEngine.Debug.DrawRay(vector, Vector2.down * distance, Color.yellow);
        UnityEngine.Debug.DrawRay(vector2, Vector2.down * distance, Color.yellow);
        UnityEngine.Debug.DrawRay(vector3, Vector2.down * distance, Color.yellow);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, Vector2.down, distance, groundMask);
        RaycastHit2D raycastHit2D2 = Physics2D.Raycast(vector2, Vector2.down, distance, groundMask);
        RaycastHit2D raycastHit2D3 = Physics2D.Raycast(vector3, Vector2.down, distance, groundMask);
        return raycastHit2D.collider != null || raycastHit2D2.collider != null || raycastHit2D3.collider != null;
    }
    private void CheckOnWall()
    {
        if (this.CheckForWall() && !this.playerState.isGhost)
        {
            this.playerState.onWall = true;
            
        }
        else
        {
            this.playerState.onWall = false;
            this.playerState.wallGrip = false;
            //this.playerState.wallJumping = false;
            this.playerState.wallSliding = false;
            this.playerState.wallClimbing = false;
        }
    }
    public bool CheckForWall()
    {
        Vector2 vector = this.col2D.bounds.center;
        UnityEngine.Debug.DrawRay(vector, Vector2.right * raycastLength * direction, Color.yellow);
        RaycastHit2D wallCollider = Physics2D.Raycast(vector, Vector2.right, raycastLength * direction, groundMask);
        return wallCollider.collider != null;
    }



    #region Mouse
    private void OnMouseDrag()
    {
        if (this.playerState.isGhost && this.playerState.canFling)
        {
            float pull = Vector2.Distance(worldMousePos, mouseDownPos);
            normalizedPull = Mathf.Clamp(Helpers.Map(pull, 0, maxPull, 0, 1), 0, 1);
            ghostDirection = (worldMousePos - mouseDownPos).normalized;
            transform.position = anchorPosition + (ghostDirection * curve.Evaluate(normalizedPull) * maxPull);
        }
    }
    private void OnMouseDown()
    {
        if (this.playerState.isGhost && this.playerState.canFling)
        {
            mouseDownPos = GetMouse();
        }
    }
    private void OnMouseUp()
    {
        if (this.playerState.isGhost && this.playerState.canFling)
        {
            rBody.bodyType = RigidbodyType2D.Dynamic;
            rBody.velocity = maxSpeed * normalizedPull * -ghostDirection;
            
        }
    }
    private void GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
    }
    private Vector3 GetMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return worldMousePos;
    }
    #endregion

    public IEnumerator MoveTo(Vector3 targetPosition, float linearSpeed)
    {
        //yield return new WaitForSeconds(0.1f);
        
        this.rBody.velocity = Vector2.zero;
        while (this.gameObject.transform.position != targetPosition)
        {
            this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, targetPosition, linearSpeed * Time.deltaTime);
            //this.gameObject.transform.position = Vector2.SmoothDamp(transform.position, targetPosition, ref velocity, 0.5f);
            yield return null;
            
        }
        this.gameObject.transform.position = targetPosition;
        this.anchorPosition = this.transform.position;

    }

    public void MoveToAnchors()
    {
        StartCoroutine(MoveTo(spectralAnchors.transform.position, maxSpeed));
        this.rBody.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Anchor"))
        {
            spectralAnchors = collision;
            if(spectralAnchors.gameObject.GetComponentInChildren<SpectreInteractable>())
            { 
                this.playerState.canFling = !spectralAnchors.gameObject.GetComponentInChildren<SpectreInteractable>().locked;

            }
            //this.movement = Vector2.zero;
            //this.playerState.isGhost = true;
            if (this.playerState.isGhost)
            {
                this.playerState.canMove = false;
                MoveToAnchors();
                //transform.position = collision.transform.position;
                //this.rBody.bodyType = RigidbodyType2D.Kinematic;
                //this.anchorPosition = transform.position;
                //StartCoroutine(MoveTo(collision.transform.position, maxSpeed));


                //rBody.velocity = Vector2.zero;
            }
        }
        if (collision.CompareTag("Space Block"))
        {
            this.rBody.velocity = maxSpeed * normalizedPull * -ghostDirection;
        }
        if (collision.CompareTag("Death Mist"))
        {
            //StopAllCoroutines();
            this.transform.position = this.interactableSpectralAnchor.transform.position;
        }
       

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Anchor"))
        {
            this.anchorPosition = Vector2.zero;


        }
    }

}
