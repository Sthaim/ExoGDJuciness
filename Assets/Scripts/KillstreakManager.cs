using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillstreakManager : MonoBehaviour
{
    [SerializeField]
    public int m_currentKillstreak;

    private int m_maxKillstreak = 5;

    private float m_jaugeKillstreak = 0;

    [SerializeField]
    private float m_frequencePerte;

    public float m_killstreakPowerOfEnnemies;

    public int m_numberOfKill;
    
    // Start is called before the first frame update
    void Start()
    {
        m_currentKillstreak = 0;
        StartCoroutine(AutoDecrease());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AutoDecrease()
    {
        while (true)
        {
            
            if (m_jaugeKillstreak > 0)
            {
                AddToJauge(-1f);
                yield return new WaitForSeconds(m_frequencePerte);
                Debug.Log(m_jaugeKillstreak);
            }
            else if (m_currentKillstreak > 0)
            {
                m_currentKillstreak = 0;
                m_numberOfKill = 0;
                yield return null;
            }
            else
            {
                m_numberOfKill = 0;
                yield return null;
            }
        }
    }

    public void AddToJauge(float p_value)
    {
        m_jaugeKillstreak += p_value;
    }

    public void AddKill()
    {
        m_numberOfKill++;
        switch (m_numberOfKill)
        {
            case 5:
                Debug.Log("KILL STREAK DE 5: BOOOOOYA");
                m_currentKillstreak++;
                break;
            case 10:
                Debug.Log("KILL STREAK DE 10: OH YEAH");
                m_currentKillstreak++;
                break;
            case 15:
                Debug.Log("KILL STREAK DE 15: WOOOOOO");
                m_currentKillstreak++;
                break;
            case 20:
                Debug.Log("KILL STREAK DE 20: WHAT THE FUCK");
                m_currentKillstreak++;
                break;
            case 25:
                Debug.Log("KILL STREAK DE 25: GOD LIKE");
                m_currentKillstreak++;
                break;
        }
    }
}
