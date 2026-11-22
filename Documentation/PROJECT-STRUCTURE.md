# Planned Unity Project Structure

This is the intended structure for the Unity project. These folders will be created when the actual Unity project is initialized. The current GitHub repository is still documentation-only.

```text
CAR-DRIVE-PARKING/
├── Assets/
│   ├── Art/
│   │   ├── Materials/
│   │   ├── Models/
│   │   ├── Textures/
│   │   └── UI/
│   ├── Audio/
│   ├── Prefabs/
│   │   ├── Vehicles/
│   │   └── Environment/
│   ├── Scenes/
│   │   └── Prototype/
│   ├── Scripts/
│   │   ├── Core/
│   │   ├── Vehicle/
│   │   ├── Camera/
│   │   ├── Input/
│   │   ├── Parking/
│   │   └── UI/
│   ├── ScriptableObjects/
│   │   └── Vehicles/
│   └── Settings/
├── Documentation/
├── Packages/
├── ProjectSettings/
└── README.md
```

## Organization rules
- Keep gameplay scripts grouped by feature.
- Keep vehicle specifications separate from controller logic.
- Keep scenes small and named clearly.
- Use prefabs for reusable vehicles and environment objects.
- Store source assets in predictable folders.
- Do not manually create Unity-generated project files; let Unity Editor initialize them.
