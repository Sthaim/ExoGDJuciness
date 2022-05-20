using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillstreakManager : MonoBehaviour
{
    private int m_currentKillstreak;

    private int m_maxKillstreak = 5;

    private float m_jaugeKillstreak = 0;

    [SerializeField]
    private float m_frequencePerte;

    public float m_killstreakPowerOfEnnemies;
    
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
                yield return null;
            }
            else
            {
                yield return null;
            }
        }
    }

    public void AddToJauge(float p_value)
    {
        m_jaugeKillstreak += p_value;
    }
}
