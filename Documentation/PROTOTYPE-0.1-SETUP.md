# Prototype 0.1 — Unity Setup Guide

This guide is for the first time the project is opened in Unity Editor. The current scripts are source files; the playable scene and vehicle prefab still need to be assembled in the Unity Editor.

## 1. Create the Unity project
1. Install Unity Hub on a Windows or Mac computer.
2. Install a supported Unity 6 LTS Editor with the 3D (URP) template.
3. Create a project named CAR-DRIVE-PARKING.
4. Copy this repository's Assets/Scripts folder into the Unity project's Assets folder.
5. Wait for Unity to compile the scripts.

## 2. Generate the temporary test scene
1. Create an empty GameObject in the scene named PrototypeBootstrap.
2. Add the PrototypeSceneBuilder component from Assets/Scripts/Core.
3. Press Play. The script creates a temporary flat test ground, a blue blockout sedan, wheel colliders, cameras and a marked parking bay.
4. Use WASD or arrow keys to drive and C to switch camera.
5. This is a runtime blockout. It is not yet a saved Unity scene or final car model.

## 3. Input setting
The first keyboard prototype uses Unity's built-in Input.GetKey API.
In Player Settings, set **Active Input Handling** to **Both** (or **Input Manager (Old)**), then restart the Editor if Unity asks.

## 4. Build a simple test vehicle
1. Create a root GameObject named TestCar.
2. Add a Rigidbody. Start with mass around 1400 kg.
3. Add the VehicleInput and VehicleController components to the root.
4. Create four child GameObjects for WheelColliders, positioned at the wheel locations. Each WheelCollider should point down its local Y axis.
5. Assign all four WheelColliders in VehicleController.
6. Add visible wheel meshes as children and assign them to the matching mesh fields.
7. Adjust WheelCollider radius to match the visible tires.
8. Make sure the body has a collider and does not intersect the ground.

## 5. Camera
1. Add an exterior Camera behind and above the car.
2. Add an interior Camera near the driver's head position.
3. Add CameraRig to a scene object and assign both cameras.
4. Press C in Play Mode to switch views.

## 6. Test ground and parking bay
1. Create a large Plane or flat road mesh.
2. Add a BoxCollider trigger for a parking bay.
3. Add ParkingZone to the trigger object.
4. Keep the trigger's BoxCollider set to Is Trigger.
5. Drive into the marked area, stop and align the car with the parking bay's forward direction.

## 7. Mobile touch controls
1. Create a Canvas with five UI Buttons: Left, Right, Throttle, Brake and Handbrake.
2. Add MobileHoldButton to each button.
3. Assign the car's VehicleInput component to Target Input.
4. Select the matching control for each button.
5. Ensure the scene has an EventSystem.
6. A camera-switch button can call CameraRig.ToggleCamera from its Button OnClick event.

## 8. First test checklist
- [ ] Car stays on the ground.
- [ ] A/D or left/right arrows steer.
- [ ] W/up accelerates.
- [ ] S/down brakes while moving forward and reverses once nearly stopped.
- [ ] Space applies the handbrake.
- [ ] C switches camera.
- [ ] Parking zone reports success only when the car is stopped and aligned.

## Important limitations
This is a starter controller, not finished realistic vehicle physics. WheelCollider setup and tuning are required in the Editor. There is no vehicle model, finished scene, sound, damage, traffic, save system or multiplayer yet. Test and tune on target hardware before treating it as a release build.
