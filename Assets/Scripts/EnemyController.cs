using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using FMPUtils.Extensions;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float Health;
    public float MaxSpeed;
    public float AccelerationRate;

    // Private Variables
    float Speed;
    float DriftFactor;
    GameObject Player;
    Vector2 PlayerDirection;
    Vector2 PreviousPlayerDirection;
    Rigidbody2D rb;
    BoxCollider2D col;
    KillstreakManager m_killstreakManager;
    [SerializeField] private LayerMask mask;
    private int delay = 0;

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
    private bool doneBound = false;
    private bool m_pastInTheGround = false;

    private float m_step = 0.03f;
    private float m_redValue;
    private SpriteRenderer m_sR;
    
    [SerializeField]
    FMODUnity.EventReference m_fmodEvent;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        m_sR = GetComponent<SpriteRenderer>();
        m_redValue = m_sR.color.r;
        Player = GameObject.FindWithTag("Player");
        m_killstreakManager = FindObjectOfType<KillstreakManager>();
        DriftFactor = 1;
    }

    void Update()
    {
        rb.velocity = new Vector2(transform.forward.z * DriftFactor * Speed * Time.fixedDeltaTime, rb.velocity.y);
        
        if (InTheGround() == true && m_pastInTheGround == true && doneBound == false)
        {
            Debug.Log("Je suis en vol");
            goalPosition = 0.3f;
            goalPosition2 =1f;
            StartCoroutine(timeReset());
            doneBound = true;
        }

        if (InTheGround() == false && m_pastInTheGround == false)
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
        // }else if (InTheGround() == false && m_pastInTheGround == false && delay < 20)
        // {
        //     delay++;
        // }
        // else
        // {
        //     delay = 0;
        // }


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
        
        //Should I rotate towards Player ?
        PlayerDirection = Player.transform.position - transform.position;
        if(Mathf.Sign(PlayerDirection.x) != Mathf.Sign(PreviousPlayerDirection.x))
        {
            RotateTowardsPlayer();
        }
        PreviousPlayerDirection = PlayerDirection;

        //Go towards Player
        

        //Die
        if(Health <= 0)
        {
            Destroy(gameObject);
            
            m_killstreakManager.AddToJauge(100);
            m_killstreakManager.AddKill();
            FMODUnity.RuntimeManager.PlayOneShotAttached(m_fmodEvent,gameObject);

        }

        if(Speed <= 0)
        {
            StartCoroutine(GetToSpeed(MaxSpeed));
        }
        
        //Debug.Log(Speed);
    }
    
    IEnumerator timeReset()
    {
        Debug.Log("Je suis rentré dans le time reset");
        yield return new WaitForSeconds(0.2f);
        goalPosition2 = 0.5f;
        goalPosition = 0.5f;
    }

    public void GetDamage(float dmg)
    {
        Health -= dmg;
    }

    void RotateTowardsPlayer()
    {
        if (PlayerDirection.x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        DriftFactor = -1;
        StartCoroutine(GetToSpeed(0));
    }

    IEnumerator GetToSpeed( float s)
    {
        //Debug.Log(s);
        float baseSpeed = Speed;
        float SignMultiplier = Mathf.Sign(s - Speed);
        for(float f=baseSpeed; f*SignMultiplier<=s; f += AccelerationRate*SignMultiplier)
        {
            Speed = f;
            yield return null;
        }
        DriftFactor = 1;
    }
    
    bool InTheGround()
    {
        // Make sure you set the ground layer to the ground
        RaycastHit2D[] ray;

        if (transform.rotation.y == 0)
        {
            Vector2 position = new Vector2(col.bounds.center.x - col.bounds.extents.x, col.bounds.min.y);
            ray = Physics2D.RaycastAll(position, Vector2.down, col.bounds.extents.y + 0.2f, mask);
        }
        else
        {
            Vector2 position = new Vector2(col.bounds.center.x + col.bounds.extents.x, col.bounds.min.y);
            ray = Physics2D.RaycastAll(position, Vector2.down, col.bounds.extents.y + 0.2f, mask);
        }

        if (ray.Length > 0)
        {
            foreach (var dip in ray)
            {
                if (ReferenceEquals(dip.collider.gameObject,gameObject)==false) return true;
            }
        }
        return false;
    }

    public void StartHitFX()
    {
        m_sR.color = new Color(1,m_sR.color.g,m_sR.color.b);
        Debug.Log("Lancement de la coroutine de flash rouge");
        StartCoroutine(HitFX());
    }
    
    private IEnumerator HitFX()
    {
        yield return null;
        Debug.Log(m_sR.color.r);
        m_sR.color = new Color(m_sR.color.r - (m_step % 255),m_sR.color.g,m_sR.color.b);
        
        if (m_sR.color.r > m_redValue) 
            StartCoroutine(HitFX());
    }
} 
