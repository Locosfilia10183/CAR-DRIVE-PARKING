# Neustadt – District 01 blockout

The first city area is a procedural greybox generated at runtime. It gives us a consistent scale reference before we spend time on final models and textures.

## Build it in Unity

1. Open the Unity project on the Windows PC.
2. Create a new scene named `Neustadt_District_01`.
3. Create an empty GameObject named `NeustadtMapBootstrap`.
4. Add the `NeustadtMapBuilder` component.
5. Press Play. The district is generated automatically.

## Included in the first blockout

- A north-south main avenue and east-west cross street
- A central four-way intersection with stop lines and zebra crossings
- Sidewalks and lane markings
- A small public parking lot with marked bays
- Ten placeholder buildings: apartments, office, cafe, dealership and workshop
- A small neighbourhood park with paths and a pond
- Basic streetlights and trees

## Coordinate plan

- Main intersection: world origin (0, 0, 0)
- Main avenue: along the Z axis
- Cross street: along the X axis
- Public parking: south-east block
- Neighbourhood park: north-west area

## Important

This is a greybox, not a finished city. Building windows, lamps, trees and road markings are simple primitives. The district has not been compiled or tested in Unity yet. We will test scale, collisions, road width and camera framing on the PC before adding more city blocks. The current menu's Play button will be connected to this scene after the first map pass is reviewed.
