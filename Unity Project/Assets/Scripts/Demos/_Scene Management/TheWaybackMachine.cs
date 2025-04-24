using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WaybackMachine : InputHandler_Generic
{
    [SerializeField] private string sceneTarget;
    bool goToScene = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GoToScene();
        }
        if (goToScene) {
            goToScene = false; // Reset the flag
            GoToScene();
        }
        
    }
    protected override void InputHandler(string command, float velocity, int note, long time) {
        if (command.Equals("play") && note == 71) {
            goToScene = true;
        }
    }

    void GoToScene() {
        SceneChanger.GoToScene(sceneTarget);
    }
}
