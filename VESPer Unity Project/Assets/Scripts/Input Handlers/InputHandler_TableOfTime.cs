using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class TableOfTime : InputHandler_Generic
{
    public TextMeshProUGUI number_text;
    public Slider tempo_slider;
    public GameObject metPing;
    public int tempo = 80;
    int flag = -1;
    double ioi;
    double startDelay = 2.0d;
    double nextLoadTime;
    int[] valid_nums = //{2, 3, 4, 5, 6, 7, 8, 9, 10};
    //{2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
    {3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
    List<GameObject> sources = new List<GameObject>();
    List<double> pingEventTimes = new List<double>();
    double killTime = 3d;
    double referenceTime;
    double buffer = 1.5d;
    public GameObject snareNote;
    int subdivision = 1;
    int next_subdivision = 1;
    public bool gamemode = false;
    double pctError = .2d;
    public double latency = 0.01d;
    public TMP_Dropdown dropdown;
    public GameObject tendencyOrb;
    List<double> closenesses;
    
    // Start is called before the first frame update
    
    void Start()
    {
        OSCSetup(); // (See note in InputHandler_Generic about OSCSetup)
        
        tempo_slider.value = tempo;
        ioi = 60d / tempo; // Converts from BPM to interonset interval (in seconds)
        closenesses = new List<double>();
    }

    public void UpdateSubdivision() {
        subdivision = dropdown.value + 1;
    }

    // FixedUpdate is called 50 times per second, regardless of frame rate.
    // We use it here because it is better for processes that are closely
    // tied to timing.
    void FixedUpdate()
    {
        foreach (var closeness in closenesses) {
            tendencyOrb.GetComponent<TendencyController>().newHit(-closeness);
        }
        closenesses.Clear();        
        
        
        tempo = (int) tempo_slider.value;
        ioi = 60d / tempo; // Converts from BPM to interonset interval (ms)

        if (flag == -1) {
            flag = 1;
            referenceTime = AudioSettings.dspTime + startDelay;
            LoadBeats_Vocal(4, referenceTime);
            number_text.enabled = false;
        }

        if (AudioSettings.dspTime > nextLoadTime) {
            int tmp = 4;
            if (gamemode) {
                tmp = 8;
            }
            LoadBeats(tmp, referenceTime);
        }

        for (int i = sources.Count - 1; i >= 0; i --) {
            if (pingEventTimes[i] < AudioSettings.dspTime) {
                GameObject sourceToKill = sources[i];
                sources.RemoveAt(i);
                pingEventTimes.RemoveAt(i);
                Destroy(sourceToKill, (float) killTime);
            }
        }
        number_text.text = subdivision.ToString(); // This is inefficient but reliable
    }

    void LoadBeats (int numBeats, double reference) {
        for (int i = 0; i < numBeats; i ++) {
            AddNewPingEvent((i == 0 ? 0 : 1), reference + ioi * i);
        }
        AddNewPingEvent(7, reference);
        SetTimes(reference + ioi * numBeats);
        if (gamemode) {
            subdivision = next_subdivision;
            
            // Generate a new valid subdivision without repeats
            List<int> tmp = valid_nums.ToList<int>();
            tmp.Remove(subdivision);
            next_subdivision = tmp[UnityEngine.Random.Range(0, tmp.Count)];
            number_text.enabled = true;
            AddNewPingEvent(6, AudioSettings.dspTime);
        }
    }

    void LoadBeats_Vocal (int numBeats, double reference) {
        for (int i = 0; i < numBeats; i ++) {
            AddNewPingEvent(i + 2, reference + ioi * i);
        }
        SetTimes(reference + ioi * numBeats);
        if (gamemode) {
            subdivision = next_subdivision;
            List<int> tmp = valid_nums.ToList<int>();
            tmp.Remove(subdivision);
            next_subdivision = tmp[UnityEngine.Random.Range(0, tmp.Count)];
            // Sound here not needed really?
            number_text.enabled = true;
        }
    }
    void SetTimes(double time) {
        nextLoadTime = time - buffer;
        referenceTime = time;
    }

    void AddNewPingEvent(int ping, double time) {
        GameObject myPing = Instantiate(metPing);
        myPing.transform.parent = transform;
        sources.Add(myPing);
        pingEventTimes.Add(time);
        myPing.GetComponent<MetPingData>().SetPing(ping, time);
    }

    // We handle input in Update rather than FixedUpdate because ideally
    // the frame rate should be faster than 50 times per second, giving us
    // a higher resolution for user input.  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            InputHandler("play", 0.5f, 60, DateTime.Now.Ticks);
        }
    }
    protected override void InputHandler(string command, float velocity, int note, long time) {
        if (!Equals(command, "play"))
        {
            return;
        }
        double myTime = AudioSettings.dspTime;
        double diff = referenceTime - (myTime - latency); // This order because referenceTime is always in the future
        double howClose = diff / (ioi / subdivision) % 1; // %1 is cursed but it does work sooooooo
        if (howClose < pctError || howClose > 1 - pctError) {
            // Note performed successfully
            Debug.Log("[✓]");
        }
        else {
            // Note missed
            Debug.Log("[x]");
        }
        if (howClose > 0.5) {
            howClose = howClose - 1;
        }
        closenesses.Add(-howClose);
    }
}
