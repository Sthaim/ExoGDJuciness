using FMPUtils.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // PLEASE READ THE GUIDE BEFORE USING THE SCRIPT //

    [Space(10)]
    [Header("Movement")]
    public float Speed = 450;
    public bool RotateToDirection = true; // Rotate To The Movement Direction
    public bool RotateWithMouseClick = false; // Rotate To The Direction Of The Mouse When Click , Usefull For Attacking

    [Space(10)]
    [Header("Jumping")]
    public float JumpPower = 22; // How High The Player Can Jump
    public float Gravity = 6; // How Fast The Player Will Pulled Down To The Ground, 6 Feels Smooth
    public int AirJumps = 1; // Max Amount Of Air Jumps, Set It To 0 If You Dont Want To Jump In The Air
    public LayerMask groundLayer; // The Layers That Represent The Ground, Any Layer That You Want The Player To Be Able To Jump In

    [Space(10)]
    [Header("Dashing")]
    public float DashPower = 3; // It Is A Speed Multiplyer, A Value Of 2 - 3 Is Recommended.
    public float DashDuration = 0.20f; // Duration Of The Dash In Seconds, Recommended 0.20f.
    public float DashCooldown = 0.5f; // Duration To Be Able To Dash Again.
    public bool AirDash = true; // Can Dash In Air ?

    [Space(10)]
    [Header("Attacking")]
    public GameObject BulletPrefab;

    // Private Variables
    bool canMove = true;
    bool canDash = true;

    bool m_pastInTheGround = false;

    [Space(10)]
    [Header("Parametre ressort")]
    
    public float position = 0.5f;
    public float velocity = 0;
    public float goalPosition = 0.5f;
    public float goalPosition2 = 0.5f;
    public float position2 = 0.5f;
    public float velocity2 = 0;
    public float angularFrequency = 0;
    public float dampingRatio = 0;

    bool doneBound = false;

    [SerializeField]
    GameObject m_test;

    float MoveDirection;
    int currentJumps = 0;
 
    Rigidbody2D rb;
    BoxCollider2D col; // Change It If You Use Something Else That Box Collider, Make Sure You Update The Reference In Start Function


    ////// START & UPDATE :

    void Start()
    {
        canMove = true;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        rb.gravityScale = Gravity;

    }   
    void Update()
    {
        if (InTheGround() == true && m_pastInTheGround == true && doneBound == false)
        {
            goalPosition = 0.3f;
            goalPosition2 =1f;
            StartCoroutine(timeReset());
            doneBound = true;
            
        }
        if (InTheGround() == false)
        {
            doneBound = false;
            if (rb.velocity.y > 7 || rb.velocity.y < -7)
            { 
                goalPosition = 0.7f;
                goalPosition2 = 0.2f;
            }
            else
            {
                goalPosition = 0.5f;
                goalPosition2 = 0.5f;
            }
        }


        m_pastInTheGround = InTheGround();
        float deltaTime = Time.deltaTime;

        SpringMotion.CalcDampedSimpleHarmonicMotion(
            ref position,ref velocity,
            goalPosition,deltaTime,
            angularFrequency,dampingRatio
            );

        SpringMotion.CalcDampedSimpleHarmonicMotion(
            ref position2, ref velocity2,
            goalPosition2, deltaTime,
            angularFrequency, dampingRatio
            );

        transform.localScale = new Vector3(position2, position, 1);
        // Get Player Movement Input
        MoveDirection = (Input.GetAxisRaw("Horizontal")); 
        // Rotation
        RotateToMoveDirection();

        // Rotate and Attack When Click Left Mouse Button
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RotateToMouse();
            Attack();
        }
        

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (MoveDirection != 0 && canDash)
            {
                if (!AirDash && !InTheGround())
                    return;


                StartCoroutine(Dash());
            }
        }
    }
    void FixedUpdate()
    {
        Move();
    } 

    IEnumerator timeReset()
    {
        yield return new WaitForSeconds(0.2f);
        goalPosition2 = 0.5f;
        goalPosition = 0.5f;
    }

    ///// MOVEMENT FUNCTIONS :

    void Move()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(MoveDirection * Speed * Time.fixedDeltaTime, rb.velocity.y);
        }

    } 
    bool InTheGround()
    {
        // Make sure you set the ground layer to the ground
        RaycastHit2D ray;

         if (transform.rotation.y == 0)
         {
            Vector2 position = new Vector2(col.bounds.center.x - col.bounds.extents.x, col.bounds.min.y);
             ray = Physics2D.Raycast(position, Vector2.down, col.bounds.extents.y + 0.2f, groundLayer);
         }
         else
         {
            Vector2 position = new Vector2(col.bounds.center.x + col.bounds.extents.x, col.bounds.min.y);
            ray = Physics2D.Raycast(position, Vector2.down, col.bounds.extents.y + 0.2f, groundLayer);
         }       

        if (ray.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Jump()
    {

        if (InTheGround())
        {
            rb.velocity = Vector2.up * JumpPower;
        }
        else
        {
            if (currentJumps >= AirJumps)
                return;

            currentJumps ++;
            rb.velocity = Vector2.up * JumpPower;
        }

    }

    void Attack()
    {
        Instantiate(BulletPrefab, transform.position, transform.rotation);
    }

    void RotateToMoveDirection()
    {
        if (!RotateToDirection)
            return;

        if (MoveDirection != 0 && canMove)
        {
            if (MoveDirection > 0)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
                
            }
            else
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
        }
    }

    ///// SPECIAL  FUNCTIONS : 

    // Multiply The Speed With Certain Amount For A Certain Duration
    IEnumerator Dash()
    {
        canDash = false;
        float originalSpeed = Speed; 
       
        Speed *= DashPower;
        rb.gravityScale = 0f; // You can delete this line if you don't want the player to freez in the air when dashing
        rb.velocity = new Vector2(rb.velocity.x, 0);

        //  You Can Add A Camera Shake Function here

        yield return new WaitForSeconds(DashDuration); 

        rb.gravityScale = Gravity;
        Speed = originalSpeed;

        yield return new WaitForSeconds(DashCooldown - DashDuration);

        canDash = true;
    }

    // Make Player Facing The Mouse Cursor , Can Be Called On Update , Or When The Player Attacks He Will Turn To The Mouse Direction
    void RotateToMouse()
    {
        if (!RotateWithMouseClick)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector2 myPos = transform.position;

        Vector2 dir = mousePos - myPos;  

        if (dir.x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        
    }

    // Reset Jump Counts When Collide With The Ground
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            RaycastHit2D ray;
            ray = Physics2D.Raycast(col.bounds.center, Vector2.down, col.bounds.extents.y + 0.2f, groundLayer);

            if (ray.collider != null)
            {
                currentJumps = 0;
            }

        }
    }
}
