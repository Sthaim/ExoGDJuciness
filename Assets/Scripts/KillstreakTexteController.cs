using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMPUtils.Extensions;
using UnityEngine.UI;

public class KillstreakTexteController : MonoBehaviour
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
    
    public float position4 = 0f;
    public float velocity4 = 0;
    public float goalPosition4 = 0;
    
    
    public float angularFrequency = 0;
    public float dampingRatio = 0;

    private float m_addedAngle;

    private int instanceDisparition;

    private RawImage m_rawImage;
    
    [SerializeField]
    private Sprite[] listSprite;
    // Start is called before the first frame update
    void Start()
    {
        goalPosition = 0;
        
        goalPosition2 = 0;
        
        goalPosition3 = 1;
        
        goalPosition4 = 0;

        m_rawImage = GetComponent<RawImage>();
        gameObject.SetActive(false);
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

        SpringMotion.CalcDampedSimpleHarmonicMotion(
            ref position4,ref velocity4,
            goalPosition4,deltaTime,
            angularFrequency,dampingRatio
        );
        position3 = Mathf.Clamp(position3, 0, Mathf.Infinity);
        transform.localPosition = new Vector3(position, position2, 1);
        transform.localScale = new Vector3(position3,position3,1);
        transform.rotation = Quaternion.Euler(new Vector3(1,1,position4));
    }

    public void LaunchTxt(int p_currentKillstreak)
    {
        gameObject.SetActive(true);
        m_rawImage.texture = listSprite[p_currentKillstreak-1].texture;
        StartCoroutine(Dissapear());
        RandomVelocity(p_currentKillstreak);
        RandomAngle(p_currentKillstreak);
    }

    void RandomVelocity(int p_currentKillstreak)
    {
        var angle = Random.Range(0,359);
        var velociti = Mathf.Cos(Mathf.Deg2Rad*angle)*100*p_currentKillstreak;
        var velociti2 = Mathf.Sin(Mathf.Deg2Rad*angle)*100*p_currentKillstreak;
        velocity = velociti;
        velocity2 = velociti2;
    }

    void RandomAngle(int p_currentKillstreak)
    {
        goalPosition3 = 1 + p_currentKillstreak*0.1f;
        position3 = 0;
        var prout = Random.Range(-30f, 30f);
        if (prout < 5 && prout > -5)
        {
            prout = 5;
        }

        position4 = prout;
        goalPosition4 = -prout;
    }

    IEnumerator Dissapear()
    {
        instanceDisparition ++;
        yield return new WaitForSeconds(3.5f);
        goalPosition3 = 0;
        yield return new WaitForSeconds(0.5f);
        if (instanceDisparition <= 1)
            gameObject.SetActive(false);
        instanceDisparition --;
        
    }
}
