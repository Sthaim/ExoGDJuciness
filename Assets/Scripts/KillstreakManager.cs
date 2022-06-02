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

    private PlayerController m_playerController;

    private KillCounterController m_killCounter;



    // Start is called before the first frame update
    void Start()
    {
        m_currentKillstreak = 0;
        m_killCounter = FindObjectOfType<KillCounterController>();
        m_playerController = FindObjectOfType<PlayerController>();
        m_jaugeController = FindObjectOfType<JaugeController>();
        StartCoroutine(AutoDecrease());
        
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
                m_playerController.ChangeShapePlayer(0);
                m_killCounter.NewKill();
                yield return null;
            }
            else
            {
                m_numberOfKill = 0;
                m_jaugeController.ColorChange(Color.white);
                m_playerController.ChangeShapePlayer(0);
                m_killCounter.NewKill();
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
        m_killCounter.NewKill();
        switch (m_numberOfKill)
        {
            case 5:
                Debug.Log("KILL STREAK DE 5: BOOOOOYA");
                m_currentKillstreak++;
                m_jaugeController.ColorChange(Color.green);
                m_playerController.ChangeShapePlayer(1);
                break;
            case 10:
                Debug.Log("KILL STREAK DE 10: OH YEAH");
                m_currentKillstreak++;
                m_jaugeController.ColorChange(Color.yellow);
                m_playerController.ChangeShapePlayer(2);
                break;
            case 15:
                Debug.Log("KILL STREAK DE 15: WOOOOOO");
                m_currentKillstreak++;
                m_jaugeController.ColorChange(new Color(1,0.5f,0));
                m_playerController.ChangeShapePlayer(3);
                break;
            case 20:
                Debug.Log("KILL STREAK DE 20: WHAT THE FUCK");
                m_currentKillstreak++;
                m_jaugeController.ColorChange(Color.red);
                m_playerController.ChangeShapePlayer(4);
                break;
            case 25:
                Debug.Log("KILL STREAK DE 25: GOD LIKE");
                m_currentKillstreak++;
                m_jaugeController.ColorChange(new Color(0.5f,0,1));
                m_playerController.ChangeShapePlayer(5);
                break;
        }
    }
}