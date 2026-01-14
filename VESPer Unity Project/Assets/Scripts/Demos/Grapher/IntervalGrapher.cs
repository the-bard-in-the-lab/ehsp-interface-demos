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
    long referencetime;
    double tickConversionFactor = 10000000.0;

    //public double scrollSpeed = 1f;
    
    void Start()
    {
        // TODO: Reference time should be the OSC stopwatch start time
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
        double dspTime = (DateTime.Now.Ticks - referencetime) / tickConversionFactor;
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
        impulses.Add((time - referencetime) / tickConversionFactor);
    }
    public void AddNewVelocity(float vel) {
        vels.Add(vel);
    }
}
