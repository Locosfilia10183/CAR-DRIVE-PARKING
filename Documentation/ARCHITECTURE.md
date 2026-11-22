# Technical Architecture

## 1. Current status
This is the planned architecture, not an implemented system. The first deliverable is an offline Unity prototype.

## 2. Client
- Engine: Unity 6 LTS
- Language: C#
- Rendering: Universal Render Pipeline (URP)
- Input: Unity Input System
- Physics: Unity physics with a vehicle controller built and tuned for the project
- UI: Unity UI Toolkit or uGUI, selected during prototype setup

The client must support scalable quality settings so mobile devices can use reduced effects and geometry while PC can use higher settings.

## 3. Prototype architecture
Keep the first prototype modular:
- Vehicle: vehicle configuration and runtime state
- VehicleInput: receives keyboard or touch input
- VehicleController: translates input into vehicle movement
- CameraRig: switches between interior and exterior cameras
- ParkingZone: detects and evaluates parking
- GameBootstrap: starts the prototype scene

Avoid coupling the vehicle physics directly to a specific input device.

## 4. Future online architecture
A possible later architecture:
- Unity game client
- Node.js + TypeScript API
- PostgreSQL persistent database
- Dedicated authoritative multiplayer session servers

The final networking technology and hosting provider must be selected after a small technical evaluation.

## 5. Server authority and security
The client must not be trusted to decide:
- Currency balances
- Vehicle ownership
- Purchases and sales
- Player-to-player trades
- Reward grants
- Competitive event results

The server should validate requests and persist important state. Never put database passwords, private API keys or server secrets in the Unity client or public repository.

## 6. Data design principles
- Use stable IDs for vehicles, upgrades and missions.
- Keep tunable vehicle specifications in data assets.
- Version save data so future updates can migrate it.
- Separate local prototype saves from future online accounts.
- Validate all data received from network clients.

## 7. Performance
Mobile is a primary target:
- Use LODs for vehicles and environment assets.
- Limit real-time lights and expensive shadows.
- Use occlusion and distance-based detail where appropriate.
- Profile on actual target devices.
- Keep physics and network update rates under control.
- Provide graphics quality presets.

## 8. Repository and project workflow
- Commit source assets and project settings.
- Do not commit Unity's generated Library, Temp, Obj or build output folders.
- Keep large binary assets optimized and organized.
- Use clear, small commits.
- Do not store secrets in Git.
