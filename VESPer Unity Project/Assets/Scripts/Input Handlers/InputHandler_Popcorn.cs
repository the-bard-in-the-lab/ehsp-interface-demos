using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PopcornHandler : InputHandler_Generic
{

    [SerializeField] float forceScalar = 50f;
    [SerializeField] float radius = 4f; // The radius within which we spawn new popcorn pieces
    [SerializeField] Rigidbody[] zones; // The zone objects in the scene. Treated as an array in case we ever want more than two.
    [SerializeField] GameObject prefab; // The popcorn prefab
    private List<float> velocities;
    private List<int> notes;
    
    void Start()
    {
        OSCSetup(); // (See note in InputHandler_Generic about OSCSetup)
        velocities = new List<float>();
        notes = new List<int>();
    } 

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            InputHandler("play", 0.5f, 60, DateTime.Now.Ticks);
        }
    
        for (int i = 0; i < velocities.Count; i ++ ) {
            // If we are in the center of the drum, activate the center zone
            // Otherwise, use the edge
            int zone = (notes[i] == 60 ? 0 : 1);
            zones[zone].AddForce(forceScalar * velocities[i] * Vector3.up);
            
            // Spawn a new piece of popcorn
            Transform spawnerTransform = GameObject.Find("Spawner").transform;
            GameObject newPrefab = Instantiate(prefab, spawnerTransform);
            Vector2 unitpos = UnityEngine.Random.insideUnitCircle;
            newPrefab.transform.position = spawnerTransform.position + new Vector3(unitpos.x * radius, 0, unitpos.y * radius);
            newPrefab.transform.rotation = UnityEngine.Random.rotationUniform;
            newPrefab.SetActive(true);   
        }
        
        velocities.Clear();
        notes.Clear();
    }

    protected override void InputHandler(string command, float velocity, int note, long time) {
        if (command.Equals("play")) {
            velocities.Add(velocity);
            notes.Add(note);
        }
    }
}

