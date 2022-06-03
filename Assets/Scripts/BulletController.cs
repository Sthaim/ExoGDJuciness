using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float Damage;
    public float Speed;
    
    [SerializeField]
    FMODUnity.EventReference m_hitEvent;

    [SerializeField] private Sprite[] m_bulletSprites;
    
    // Private Variables
    Rigidbody2D rb;

    private SpriteRenderer m_spriteRenderer;
    
    private KillstreakManager m_killstreakManager;
    
    void Start()
    {
        
        m_killstreakManager = FindObjectOfType<KillstreakManager>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.sprite = m_bulletSprites[m_killstreakManager.m_currentKillstreak];
        rb = GetComponent<Rigidbody2D>();
        var scale = 0.1f + 0.07f * m_killstreakManager.m_currentKillstreak;
        gameObject.transform.localScale= new Vector3(scale,scale,1);
        //Rotate towards Mouse
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector2 myPos = transform.position;
        Vector2 dir = mousePos - myPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        ScreenShakeController.m_instance.StartShake(angle);
        if(angle > -90 && angle <= 90)
        {
            transform.rotation = Quaternion.AngleAxis(angle, transform.forward);
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(angle, -transform.forward);
        }

        rb.velocity = transform.right * Speed * Time.fixedDeltaTime * (1+(m_killstreakManager.m_currentKillstreak*0.2f));
    }


    private void Update()
    {
        if (GetComponent<Rigidbody2D>())
        {
            Debug.Log(GetComponent<Rigidbody2D>().rotation);
            transform.up = GetComponent<Rigidbody2D>().velocity;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_hitEvent);
            EnemyController ec = collision.gameObject.GetComponent<EnemyController>();
            ec.GetDamage(Damage);
            ec.StartHitFX();
            Destroy(gameObject);
        }
    }
}
