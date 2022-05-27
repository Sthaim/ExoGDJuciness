using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillstreakManager : MonoBehaviour
{
    [NonSerialized]
    public int m_currentKillstreak;

    private int m_maxKillstreak = 5;
    
    //[NonSerialized]
    public float m_jaugeKillstreak = 0;

    [SerializeField]
    private float m_frequencePerte;

    public int m_numberOfKill;

    private JaugeController m_jaugeController;
    
    // Start is called before the first frame update
    void Start()
    {
        m_currentKillstreak = 0;
        m_jaugeController = FindObjectOfType<JaugeController>();
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
                Debug.Log("Je suis dans le addtojauge");
                yield return new WaitForSeconds(m_frequencePerte);
            }
            else if (m_currentKillstreak > 0)
            {
                m_currentKillstreak = 0;
                m_numberOfKill = 0;
                m_jaugeController.ColorChange(Color.white);
                yield return null;
            }
            else
            {
                m_numberOfKill = 0;
                m_jaugeController.ColorChange(Color.white);
                yield return null;
            }
        }
    }

    public void AddToJauge(float p_value)
    {
        m_jaugeKillstreak += p_value;
        if (m_jaugeKillstreak > 100)
            m_jaugeKillstreak = 100;

    }

    public void AddKill()
    {
        m_numberOfKill++;
        switch (m_numberOfKill)
        {
            case 5:
                Debug.Log("KILL STREAK DE 5: BOOOOOYA");
                m_currentKillstreak++;
                m_jaugeController.ColorChange(Color.green);
                break;
            case 10:
                Debug.Log("KILL STREAK DE 10: OH YEAH");
                m_currentKillstreak++;
                m_jaugeController.ColorChange(Color.yellow);
                break;
            case 15:
                Debug.Log("KILL STREAK DE 15: WOOOOOO");
                m_currentKillstreak++;
                m_jaugeController.ColorChange(new Color(1,0.5f,0));
                break;
            case 20:
                Debug.Log("KILL STREAK DE 20: WHAT THE FUCK");
                m_currentKillstreak++;
                m_jaugeController.ColorChange(Color.red);
                break;
            case 25:
                Debug.Log("KILL STREAK DE 25: GOD LIKE");
                m_currentKillstreak++;
                m_jaugeController.ColorChange(new Color(0.5f,0,1));
                break;
        }
    }
}