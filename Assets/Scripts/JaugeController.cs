using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;
using Random = UnityEngine.Random;
using FMPUtils.Extensions;

public class JaugeController : MonoBehaviour
{
    [SerializeField] private GameObject m_rightAnchor;
    
    [SerializeField] private GameObject m_leftAnchor;

    private KillstreakManager m_killstreakManager;

    [SerializeField]
    private UnityEngine.UI.Image m_childImage;
    
    [Space(10)]
    [Header("Parametre ressort")]
    public float position = 0f;
    public float velocity = 0;
    public float goalPosition = 0f;
    
    public float position2 = 0f;
    public float velocity2 = 0;
    public float goalPosition2 = 0f;
    
    public float position3 = 0f;
    public float velocity3 = 0;
    public float goalPosition3 = 0f;

    public float position4 = 0f;
    public float velocity4 = 0;
    public float goalPosition4 = 0f;
    
    public float angularFrequency = 0;
    public float dampingRatio = 0;
    
    void Start()
    {
        if (!m_childImage.sprite)
            m_childImage = GetComponentInChildren<UnityEngine.UI.Image>();
        m_killstreakManager = FindObjectOfType<KillstreakManager>();
        
        goalPosition = m_leftAnchor.transform.position.x;
        
        goalPosition2 = m_leftAnchor.transform.position.y;
        
        goalPosition3 = m_rightAnchor.transform.position.x;
        
        goalPosition4 = m_rightAnchor.transform.position.y;

        StartCoroutine(UpdatedAngles());

    }

    private void Update()
    {
        m_childImage.fillAmount = m_killstreakManager.m_jaugeKillstreak/100;

        var deltaTime = Time.deltaTime;
        
        SpringMotion.CalcDampedSimpleHarmonicMotion(
            ref position,ref velocity,
            goalPosition,deltaTime,
            angularFrequency,dampingRatio
        );
        
        SpringMotion.CalcDampedSimpleHarmonicMotion(
            ref position2,ref velocity2,
            goalPosition2,deltaTime,
            angularFrequency,dampingRatio
        );
        
        SpringMotion.CalcDampedSimpleHarmonicMotion(
            ref position3,ref velocity3,
            goalPosition3,deltaTime,
            angularFrequency,dampingRatio
        );
        
        SpringMotion.CalcDampedSimpleHarmonicMotion(
            ref position4,ref velocity4,
            goalPosition4,deltaTime,
            angularFrequency,dampingRatio
        );
        
        
        
        transform.position = new Vector2(position,position2);
        
        transform.LookAt(new Vector3(position3,position4,0));
        var rotationDummy = transform.localEulerAngles;
        transform.rotation = Quaternion.Euler(0f,0f,rotationDummy.x);
        
    }

    private IEnumerator UpdatedAngles()
    {
        while (true)
        {
            var angle = Random.Range(0,359);
            var velociti = Mathf.Cos(Mathf.Deg2Rad*angle)*100*m_killstreakManager.m_currentKillstreak;
            var velociti2 = Mathf.Sin(Mathf.Deg2Rad*angle)*100*m_killstreakManager.m_currentKillstreak;
            velocity = velociti;
            velocity2 = velociti2;
        
            var angle2 = Random.Range(0,359);
            var velociti3 = Mathf.Cos(Mathf.Deg2Rad*angle2)*100*m_killstreakManager.m_currentKillstreak;
            var velociti4 = Mathf.Sin(Mathf.Deg2Rad*angle2)*100*m_killstreakManager.m_currentKillstreak;
            velocity3 = velociti3;
            velocity4 = velociti4;
            
            yield return new WaitForSeconds(0.5f/(m_killstreakManager.m_currentKillstreak+1));
        }
    }

    public void ColorChange(Color p_color)
    {
        m_childImage.color = p_color;
    }
}
