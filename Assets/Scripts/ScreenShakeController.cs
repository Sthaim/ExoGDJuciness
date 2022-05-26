using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using FMPUtils.Extensions;
using Unity.Mathematics;

public class ScreenShakeController : MonoBehaviour
{
    public static ScreenShakeController m_instance;

    private KillstreakManager m_killstreak;

    [NonSerialized]
    public int power;
    
    [Space(10)]
    [Header("Parametre ressort")]
    public float position = 0f;
    public float velocity = 0;
    public float goalPosition = 0f;
    public float goalPosition2 = 0f;
    public float position2 = 0f;
    public float velocity2 = 0;
    public float angularFrequency = 0;
    public float dampingRatio = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        m_killstreak = FindObjectOfType<KillstreakManager>();
        m_instance = this;
        m_instance.power = m_killstreak.m_currentKillstreak;
    }

    // Update is called once per frame
    void Update()
    {
        var deltaTime = Time.deltaTime;

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
        

        transform.position = new Vector3(position, position2, -10);
    }
    

    public void StartShake(float angle)
    {
        velocity = Mathf.Cos(Mathf.Deg2Rad*angle)*m_killstreak.m_currentKillstreak*2;
        velocity2 = Mathf.Sin(Mathf.Deg2Rad*angle)*m_killstreak.m_currentKillstreak*2;
    }
}
