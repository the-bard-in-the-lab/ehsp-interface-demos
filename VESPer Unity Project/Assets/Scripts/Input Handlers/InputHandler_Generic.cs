using UnityEngine;
public class InputHandler_Generic : MonoBehaviour
{
    OSC osc;
    void Start() {
        // We always need to do OSC setup in Start(). When writing
        // classes that inherit from this one, make sure to include
        // OSCSetup() in Start() if you have a Start() method.
        OSCSetup();
	}

    /// <summary>
    /// Configures the OSC object to match global settings and
    /// connects the OSCHandler to its message. This function needs
    /// to be run when a scene starts.
    /// </summary>
    protected void OSCSetup() {
        osc = GlobalData.getOSC();
        osc.SetAddressHandler(GlobalData.OSCAddress, OSCHandler);
    }
    /// <summary>
    /// Internal function that extracts parameters from an Sensory Percussion OSC message
    /// and passes them to the input handler
    /// </summary>
    /// <param name="message">The OSC message as an object</param>
    /// <param name="time">The time (in ticks since the OSC listener began listening) the OSC message was received</param>
    void OSCHandler(OscMessage message, long time) {
        string[] msg = message.ToString().Split(" ");
		// Sensory Percussion 2 OSC messages take the form /obsidian/hwout/midi1 <command> <velocity> <note> <transpose> <channel>
        // Typically, we care about the command (play vs. stop), the velocity (0.0 to 1.0), and the note (as a MIDI number)
        string command = msg[1];
        float velocity = float.Parse(msg[2]);
        int note = int.Parse(msg[3]);
        InputHandler(command, velocity, note, time);
    }

    /// <summary>
    /// The template for all functions that handle user input from Sensory Percussion.
    /// </summary>
    /// <param name="command">The MIDI command (usually "play", "stop", or "cc")</param>
    /// <param name="velocity">The velocity (from 0.0 to 1.0)</param>
    /// <param name="note">The MIDI note number</param>
    /// <param name="time">The time (in ticks) since the OSC listener began listening</param>
    protected virtual void InputHandler(string command, float velocity, int note, long time) {
        Debug.Log("If you see this, you have a child class that needs to override this method.");
        Debug.Log($"Message: {command} {velocity} {note}");
    }
}
