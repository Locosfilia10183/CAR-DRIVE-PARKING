# Game Design Document — CAR DRIVE PARKING

## 1. Vision
Create an original open-world driving and car-life simulator where players can explore a fictional German city, improve their driving skills, park accurately, customize vehicles and eventually meet other players.

The experience should feel immersive and approachable. Vehicle behavior should be believable without making the game unnecessarily difficult, and mobile performance is a first-class requirement.

## 2. Target platforms
- Windows PC
- iOS
- Android

Cross-platform multiplayer is a long-term goal, not a requirement for the first prototype.

## 3. Core pillars
1. **Driving feel:** Responsive steering, acceleration, braking and vehicle weight.
2. **Parking skill:** Clear parking spaces, challenges and feedback.
3. **Car ownership:** Choose, maintain and personalize vehicles.
4. **Open-world freedom:** Explore a fictional German city and its surrounding roads.
5. **Social play:** Eventually meet, drive and compete with other players.

## 4. World: Neustadt
Neustadt is a fictional German city. Its planned districts are:
- Downtown / Innenstadt
- Residential district / Wohngebiet
- Industrial district / Industriegebiet
- Port / Hafen
- Commercial district / Gewerbegebiet
- Countryside / Landgebiet
- Autobahn ring

Future expansion ideas include mountain roads, an airport and additional rural routes.

## 5. Vehicles
Vehicles will use original fictional manufacturers and designs. No real manufacturer logos, badges, protected vehicle designs or unlicensed brand assets should be used.

Initial fictional brand concepts (working names only; trademark checks are required before release):
- VELTOR
- KRONEN
- NORDWERK
- STADTMOBIL

First test vehicle concept: **VELTOR V6 S**, a fictional sport sedan.

Planned vehicle variety:
- Small city cars
- Compact cars
- Sedans
- Coupes
- Sports cars
- SUVs
- Off-road vehicles

## 6. Vehicle systems
Planned systems, introduced gradually:
- Front-wheel, rear-wheel and all-wheel drive
- Automatic and manual transmission
- Steering, throttle, braking and reverse
- Handbrake
- Suspension and tire behavior
- ABS and traction control
- Engine and turbo upgrades
- Vehicle lights, indicators and interior dashboard
- Doors, hood and trunk interactions
- Exterior and interior camera views
- Vehicle audio: engine, exhaust, tires and environment
- Visual damage, with gameplay effects considered later

Vehicle upgrades should affect actual handling, not just appearance.

## 7. Controls
### PC
- Keyboard controls first
- Controller support planned

### Mobile
- On-screen steering, or optional tilt steering
- Throttle and brake
- Handbrake
- Gear selection where needed
- Adjustable control layout and sensitivity planned

## 8. Activities
Planned activities include:
- Parking challenges
- Driving school
- Delivery missions
- Taxi missions
- Time trials
- Drift events

These activities will be added after the basic driving model is reliable.

## 9. Garage and progression
Planned features:
- Vehicle selection and ownership
- Garage storage
- Vehicle customization
- Repairs and maintenance
- Dealerships
- Multiple garages in a later update

A provisional in-game currency name is **CDP Credits**. It is a working concept and may change. Players should earn progression through gameplay. The design must avoid pay-to-win mechanics.

## 10. World simulation
Later systems:
- Traffic vehicles and AI
- Pedestrians
- Day/night cycle
- Weather
- GPS and minimap
- Fuel stations
- Repair shops
- Police and traffic checks

## 11. Multiplayer vision
Long-term target:
- Up to 16 players per session
- Public and private rooms
- Friends and social features
- Car meets
- Parking, drag and drift events
- Vehicle trading with server-side validation
- Text chat; voice chat only after privacy and moderation design

Important player data such as currency, vehicle ownership, trades and event results must be validated by an authoritative server.

## 12. Prototype 0.1 — strict scope
The first prototype is intentionally small:
- One vehicle
- One test track
- Basic vehicle physics
- Steering, throttle, brake and reverse
- Exterior and interior cameras
- Keyboard input
- Basic mobile control experiment
- One marked parking zone

**Out of scope for 0.1:** open-world city, traffic, economy, accounts and multiplayer.

## 13. Quality goals
- Stable frame rate on supported mobile devices
- Scalable visual quality between mobile and PC
- Readable UI on small screens
- Fast startup and scene loading
- Consistent controls across platforms
- Clear separation between prototype code and future production systems
