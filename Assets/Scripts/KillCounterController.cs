using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMPUtils.Extensions;
using TMPro;
using UnityEngine.UI;

public class KillCounterController : MonoBehaviour
{
    [Header("Parametre ressort")]
    public float position = 0f;
    public float velocity = 0;
    public float goalPosition = 0f;
    
    public float position2 = 0f;
    public float velocity2 = 0;
    public float goalPosition2 = 0f;
    
    public float position3 = 0f;
    public float velocity3 = 0;
    public float goalPosition3 = 0;
    
    public float angularFrequency = 0;
    public float dampingRatio = 0;
    private KillstreakManager m_killstreakManager;
    
    // Start is called before the first frame update
    void Start()
    {
        m_killstreakManager = FindObjectOfType<KillstreakManager>();
        
        goalPosition = transform.localScale.x;
        
        goalPosition2 = transform.position.x;

        goalPosition3 = transform.position.y;
        
        NewKill();

        StartCoroutine(UpdatedAngles());
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
            ref position2,ref velocity2,
            goalPosition2,deltaTime,
            angularFrequency,dampingRatio
        );
        
        SpringMotion.CalcDampedSimpleHarmonicMotion(
            ref position3,ref velocity3,
            goalPosition3,deltaTime,
            angularFrequency,dampingRatio
        );
        
        transform.localScale = new Vector3(position,position,1f);
        
        transform.position = new Vector3(position2,position3,1f);
        
        
    }
    
    private IEnumerator UpdatedAngles()
    {
        while (true)
        {
            var angle = Random.Range(0,359);
            var velociti = Mathf.Cos(Mathf.Deg2Rad*angle)*100*m_killstreakManager.m_currentKillstreak;
            var velociti2 = Mathf.Sin(Mathf.Deg2Rad*angle)*100*m_killstreakManager.m_currentKillstreak;
            velocity2 = velociti;
            velocity3 = velociti2;

            yield return new WaitForSeconds(0.5f/(m_killstreakManager.m_currentKillstreak+1));
        }
    }

    public void NewKill()
    {
        if (m_killstreakManager)
        {
            if(m_killstreakManager.m_numberOfKill > 0)
                velocity = 50+50*m_killstreakManager.m_currentKillstreak;
            GetComponent<TextMeshProUGUI>().text = m_killstreakManager.m_numberOfKill.ToString();
        }
    }
}
