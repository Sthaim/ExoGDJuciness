using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMPUtils.Extensions;

public class CursorController : MonoBehaviour
{

    private KillstreakManager m_killstreakManager;
    
    public float position = 1f;
    public float velocity = 0;
    public float goalPosition = 1f;
    public float angularFrequency = 0;
    public float dampingRatio = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        m_killstreakManager = FindObjectOfType<KillstreakManager>();
        Cursor.visible = false;
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
        
        transform.position = Input.mousePosition;

        transform.localScale = new Vector3(position,position,1f);
    }

    public void AttackVelocity()
    {
        velocity = 10+10*m_killstreakManager.m_currentKillstreak;
    }
}
