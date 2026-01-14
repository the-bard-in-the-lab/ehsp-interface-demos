using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GraphHandler : InputHandler_Generic
{
    // This is the input handler for the Grapher demo.
    IntervalGrapher myGrapher;
    void Start()
    {
        OSCSetup(); // (See note in InputHandler_Generic about OSCSetup)
        myGrapher = GetComponent<IntervalGrapher>();
    } 
    void Update() {
        // Mimic drum input using the space bar
        if (Input.GetKeyDown(KeyCode.Space)) {
            InputHandler("play", 0.5f, 60, DateTime.Now.Ticks);
        }
    }

    protected override void InputHandler(string command, float velocity, int note, long time) {
        if (command.Equals("play")) {
            myGrapher.AddNewImpulse(time);
            myGrapher.AddNewVelocity(velocity);
        }
    }
}

