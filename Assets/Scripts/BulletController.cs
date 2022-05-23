using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float Damage;
    public float Speed;

    // Private Variables
    Rigidbody2D rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        
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

        rb.velocity = transform.right * Speed * Time.fixedDeltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().GetDamage(Damage);
            Destroy(gameObject);
        }
    }
}
