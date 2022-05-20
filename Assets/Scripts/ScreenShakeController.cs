using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class ScreenShakeController : MonoBehaviour
{
    public static ScreenShakeController m_instance;

    private float m_shakeTimeRemaining, m_shakePower, m_shakeFadeTime, m_shakeRotation;

    public float m_rotationMultiplier = 15f;
    
    // Start is called before the first frame update
    void Start()
    {
        m_instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        if (m_shakeTimeRemaining > 0)
        {
            m_shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * m_shakePower;
            float yAmount = Random.Range(-1f, 1f) * m_shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0f);

            m_shakePower = Mathf.MoveTowards(m_shakePower, 0f, m_shakeFadeTime * Time.deltaTime);
            
            transform.rotation = Quaternion.Euler(0f,0f,m_shakeRotation * Random.Range(-1f,1f));

            if (m_shakeTimeRemaining <= 0)
            {
                transform.position = new Vector3(0f,0f,-10f);
            }
        }
        
    }

    public void StartShake(float p_length, float p_power)
    {
        m_shakeTimeRemaining = p_length;
        m_shakePower = p_power;

        m_shakeFadeTime = p_power / p_length;

        m_shakeRotation = p_power * m_rotationMultiplier;
    }
}
