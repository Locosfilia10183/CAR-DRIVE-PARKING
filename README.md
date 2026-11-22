# CAR DRIVE PARKING

**CAR DRIVE PARKING** is an original open-world car-life, driving and parking simulator planned for Windows PC, iOS and Android.

The game aims for realistic-feeling vehicle handling with performance-conscious visuals on mobile. The first milestone is a small offline driving prototype. Multiplayer and the larger open world come later, after the driving foundation is stable.

## Current direction
- **Engine:** Unity 6 LTS
- **Rendering:** Universal Render Pipeline (URP)
- **Gameplay code:** C#
- **Initial platforms:** Windows, iOS and Android
- **Long-term goal:** Cross-platform multiplayer, up to 16 players per session
- **World:** A fictional German city called Neustadt
- **Business model:** Free-to-play direction; no pay-to-win design

## First prototype — 0.1
1. One original test vehicle
2. A simple test track and parking area
3. Steering, throttle, braking and reverse
4. Exterior and interior camera views
5. Keyboard controls on PC
6. A basic mobile touch-control test

No online multiplayer is planned for this first prototype.

## Documentation
- [Game Design Document](Documentation/GDD.md)
- [Roadmap](Documentation/ROADMAP.md)
- [Technical Architecture](Documentation/ARCHITECTURE.md)
- [Asset and IP Policy](Documentation/ASSET-POLICY.md)
- [Unity project structure](Documentation/PROJECT-STRUCTURE.md)
- [Prototype 0.1 setup guide](Documentation/PROTOTYPE-0.1-SETUP.md)
- [VELTOR V6 S vehicle specification](Documentation/VELTOR-V6-S.md)\n\n## Prototype code\n- [Procedural scene builder](Assets/Scripts/Core/PrototypeSceneBuilder.cs)\n- [Vehicle visual blockout](Assets/Scripts/Vehicle/VehicleVisualBuilder.cs)\n- [Steering wheel animation](Assets/Scripts/Vehicle/SteeringWheelAnimator.cs)

## Development status
**Phase:** Planning and foundation  
**Current milestone:** Prototype 0.1 — Driving Core  
**Status:** Not yet a playable Unity build

## Important
This repository currently contains planning and project foundation files. It is not yet a runnable Unity project. Unity Editor setup and the first playable scene will be created when a Windows or Mac development computer is available.


## Main menu prototype
- [Main menu and settings script](Assets/Scripts/Core/MainMenuBootstrap.cs)
- [Main menu setup guide](Documentation/MAIN-MENU-SETUP.md)

The first runtime-generated main menu is committed. It is a prototype scaffold and has not yet been compiled or tested in Unity. The Play button will be connected to the Neustadt game scene when the map is built.


The main menu now includes a runtime-rendered 3D VELTOR V6 S preview with a rotating vehicle blockout. Supporting script: [MenuVehicleRotation.cs](Assets/Scripts/Core/MenuVehicleRotation.cs). This remains uncompiled and untested in Unity until the project is opened on a development PC.


## Neustadt – first map blockout
- [Procedural district builder](Assets/Scripts/World/NeustadtMapBuilder.cs)
- [District 01 setup and layout](Documentation/NEUSTADT-DISTRICT-01.md)

District 01 contains a four-way intersection, avenue, sidewalks, road markings, parking bays, placeholder buildings, a small park, streetlights and trees. It is a runtime greybox and has not yet been compiled or tested in Unity.
