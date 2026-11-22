# Main Menu – Prototype 0.1

This is the first menu foundation for CAR DRIVE PARKING. It is generated at runtime with Unity's built-in UI system and does not require TextMeshPro or extra packages.

## Create the menu scene in Unity

1. Open the Unity project on your Windows PC.
2. In the Project window, open `Assets/Scripts/Core`.
3. Create an empty GameObject in the Hierarchy and name it `MainMenuBootstrap`.
4. Add the `MainMenuBootstrap` component to that object.
5. Press Play.

The menu currently includes:
- Main menu with Play, Settings, About and Exit
- Graphics quality selection
- Resolution selection
- German / English language preference storage
- Control preference selection
- Master volume slider
- Fullscreen toggle

Settings are saved locally using PlayerPrefs.

## Current limitations

- The Play button currently displays a placeholder message. The Neustadt map and playable game scene are the next development step.
- Language selection stores the preference; translated menu labels will be added later.
- Control selection stores the preference; the actual control schemes will be connected when gameplay is integrated.
- The menu is a runtime-generated prototype, not yet a polished final design.
- Add the Unity scene to Build Settings / Build Profiles when we prepare a standalone build.

## PC testing

Whenever new scripts are committed, open the project in Unity on the PC and press Play to test the current prototype. This file is a setup guide; the repository is not a complete Unity project until the Unity project files and assets are added.


## Visual update
The main menu now uses a split-screen layout: navigation on the left and a temporary VELTOR V6 S silhouette stage on the right. The right side now renders a live 3D VELTOR V6 S blockout through a RenderTexture. The car rotates slowly, and its geometry reuses the existing procedural vehicle visual builder. It is still a temporary primitive model, not a finished production vehicle. The Play button remains a placeholder until the Neustadt game scene exists.


Additional script: [MenuVehicleRotation.cs](../Assets/Scripts/Core/MenuVehicleRotation.cs) rotates the 3D preview vehicle. The menu scene creates its preview camera, lights, render texture, vehicle blockout and floor at runtime.
