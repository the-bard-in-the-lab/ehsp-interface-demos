using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalGrapher : MonoBehaviour
{
    private GraphGenerator m_Grapher;
    private double diff;
    public List<double> impulses;
    public List<float> vels;
    public List<double> iois;
    public List<Vector2> pairs;
    [SerializeField] bool useDSPTime = false;
    long referencetime;
    

    //public double scrollSpeed = 1f;
    
    void Start()
    {
        referencetime = DateTime.Now.Ticks;
        m_Grapher = GetComponent<GraphGenerator>();
        diff = m_Grapher.xMax - m_Grapher.xMin;
        impulses = new List<double>();
        vels = new List<float>();
        iois = new List<double>();
        pairs = new List<Vector2>();
    }

    void Update()
    {
        //For unknown reasons, this seems to work better with Time.time than AudioSettings.dspTime.
        //double dspTime = useDSPTime ? AudioSettings.dspTime : Time.time;
        double dspTime = (DateTime.Now.Ticks - referencetime) / 10000000.0;
        m_Grapher.xMin = dspTime;
        m_Grapher.xMax = dspTime + diff;
        
        if (iois.Count < impulses.Count - 1 && impulses.Count > 1) {
            for (int i = iois.Count; i < impulses.Count - 1; i ++) {
                iois.Add(impulses[i + 1] - impulses[i]);
            }
        }

        pairs.Clear();
        for (int i = 0; i < iois.Count; i ++) {
            pairs.Add(new Vector2((float) (impulses[i] + diff), (float) (m_Grapher.yMax - iois[i])));
        }
        m_Grapher.DrawGraph(pairs, vels);
    }
    public void AddNewImpulse(long time) {
        //double myDSP = AudioSettings.dspTime;
        //impulses.Add(useDSPTime ? AudioSettings.dspTime : Time.time);
        impulses.Add((time - referencetime) / 10000000.0);
        //Debug.Log($"diff: {myDSP - lastTime}");
    }
    public void AddNewVelocity(float vel) {
        vels.Add(vel);
    }
}
