# VESPer

![A preview image showing popcorn falling onto a snare drum.](Screenshots/Preview.png)

**VESPer** stands for Visual Expression with Sensory Percussion. It was developed for the paper Exploring Real Time Interfaces with Sensory Percussion and is intended for use alongside [EVANS Hybrid Sensory Percussion](https://sunhou.se/sensorypercussion) by Sunhouse and EVANS.

## Requirements

The Unity project was original created in version `2022.3.25f1`. It has since been updated to a Unity 6 version following security patches.

For the most complete experience, users will need the EVANS Hybrid Sensory Percussion system, the Sensory Percussion 2 software, and one snare drum.

In lieu of Sensory Percussion 2, users can use [MaxMSP](https://cycling74.com/products/max) and the provided patch to send OSC messages. The Unity project also supports the use of the space key as a drum hit in the center of the head with a velocity of `0.5` (or `64` in MIDI terms) for testing purposes.

## Quickstart

### For players and students

If you are using Sensory Percussion, follow the following sequence:
1. Turn on your Sensory Percussion unit.
2. Launch the .sp2 patch file included in this repository.
3. Launch the Unity project from the `Assets/Scenes/Demo/Menu` scene.

You can navigate using a mouse and keyboard, or using your drum to interact with menus. Hitting the shell of the drum will take you back to the main menu.

**If you do not have Sensory Percussion, you can still demo this repository!** Use the [MaxMSP](https://cycling74.com/products/max) patch included to send messages as if using a Sensory Percussion setup, or just use the space bar to simulate a drum in most scenes!

### For developers

`Assets/Scenes/Starter Pack` contains the basic elements required in a scene:
- An object with the OSC script attached (available as a prefab at `Assets/Prefabs/OSC`)
- An object with an input handler attached.
  - `InputHandler_Generic.cs` is the class from which all other input handlers inherit.
  - The function `InputHandler` is where you will process most user input. `InputHandler` takes as input the following parameters:
    - `string command`, typically `play`, `stop`, or `cc`
    - `float velocity`, which ranges from 0.0 to 1.0
    - `int note`, a MIDI note number
    - `long time`, the time in ticks since the OSC class was initialized (useful for precise timing)
    

### Unity Demos

![The main menu of the VESPer demos, featuring a 3D model of a drum in the background.](Screenshots/Menu.png)

The `VESPer Unity Project` folder is the root of the Unity project. The `Assets/Scenes/Demo/Menu` scene is the main menu of the demo project (pictured above).

To connect the Unity project with Sensory Percussion 2, you will need to synchronize the OSC port. You can do this by ensuring that the port number in the Settings menu matches the port number in Sensory Percussion 2:

![The OSC Network panel of the Sensory Percussion 2 settings. The "remote port" is 9001.](Screenshots/SP2_OSC.png)

When properly synchronized, tapping the center of your drum lightly should move between the different highlighted buttons on the main menu.

### Screenshots
![The Popcorn demo, with pieces of popcorn flying in the air.](Screenshots/Popcorn.png)

![The Grapher demo, with a line showing the user's playing. The line is colored red and blue according to the user's velocity. A slider at the botton adjusts the line thickness.](Screenshots/Grapher.png)

## Credits

(Authorship has been redacted for anonymous peer review.)

### Code

The OSC integration is adapted from [https://t-o-f.info/UnityOSC/](https://t-o-f.info/UnityOSC/). **Please note that this is no longer actively maintained.**

No generative AI was used in the creation of this repository

### Models

[Popcorn](https://www.cgtrader.com/3d-models/food/miscellaneous/popcorn-97e6ded8-8b28-48d9-8f29-cd11d2f97808) by [3Dmarkethub on cgtrader](https://www.cgtrader.com/designers/3dmarkethub)

[Snare Drum](https://sketchfab.com/3d-models/snare-low-poly-8ab75d98e94d4ef797b418ce086c5022) by [Murik.3D on Sketchfab](https://sketchfab.com/Murik.3D)