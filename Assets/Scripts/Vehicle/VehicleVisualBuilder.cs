using UnityEngine;

namespace CarDriveParking
{
    /// <summary>
    /// Adds temporary exterior and cockpit details to the VELTOR V6 S blockout.
    /// All geometry is made from Unity primitives and can later be replaced by authored meshes.
    /// Vehicle forward is local +Z and vehicle left is local -X.
    /// </summary>
    public static class VehicleVisualBuilder
    {
        public static void Build(Transform car)
        {
            Material paint = MakeMaterial(new Color(0.055f, 0.22f, 0.62f));
            Material glass = MakeMaterial(new Color(0.035f, 0.075f, 0.105f));
            Material trim = MakeMaterial(new Color(0.035f, 0.04f, 0.05f));
            Material chrome = MakeMaterial(new Color(0.62f, 0.67f, 0.72f));
            Material headlight = MakeMaterial(new Color(0.78f, 0.9f, 1f), 0.35f);
            Material taillight = MakeMaterial(new Color(0.75f, 0.025f, 0.035f), 0.25f);
            Material seat = MakeMaterial(new Color(0.055f, 0.06f, 0.075f));
            Material dashboard = MakeMaterial(new Color(0.025f, 0.03f, 0.04f));

            // Exterior body shaping: hood, trunk, roof glass and window panels.
            Part(car, PrimitiveType.Cube, "Hood", new Vector3(0f, 1.12f, 1.28f), new Vector3(1.68f, 0.13f, 1.35f), paint);
            Part(car, PrimitiveType.Cube, "Trunk lid", new Vector3(0f, 1.08f, -1.63f), new Vector3(1.62f, 0.12f, 0.72f), paint);
            Part(car, PrimitiveType.Cube, "Roof", new Vector3(0f, 1.72f, -0.18f), new Vector3(1.34f, 0.12f, 1.48f), paint);
            Part(car, PrimitiveType.Cube, "Front windshield", new Vector3(0f, 1.47f, 0.82f), new Vector3(1.30f, 0.055f, 0.78f), glass, new Vector3(22f, 0f, 0f));
            Part(car, PrimitiveType.Cube, "Rear windshield", new Vector3(0f, 1.45f, -1.05f), new Vector3(1.28f, 0.055f, 0.68f), glass, new Vector3(-24f, 0f, 0f));

            // Side windows and door details.
            for (int side = -1; side <= 1; side += 2)
            {
                float x = side * 0.755f;
                Part(car, PrimitiveType.Cube, side < 0 ? "Left front side glass" : "Right front side glass",
                    new Vector3(x, 1.48f, 0.42f), new Vector3(0.035f, 0.43f, 0.78f), glass);
                Part(car, PrimitiveType.Cube, side < 0 ? "Left rear side glass" : "Right rear side glass",
                    new Vector3(x, 1.48f, -0.55f), new Vector3(0.035f, 0.43f, 0.68f), glass);
                Part(car, PrimitiveType.Cube, side < 0 ? "Left door handle" : "Right door handle",
                    new Vector3(side * 0.94f, 1.00f, -0.12f), new Vector3(0.045f, 0.055f, 0.25f), chrome);
                Part(car, PrimitiveType.Cube, side < 0 ? "Left mirror stem" : "Right mirror stem",
                    new Vector3(side * 0.88f, 1.34f, 0.65f), new Vector3(0.22f, 0.06f, 0.12f), trim);
                Part(car, PrimitiveType.Sphere, side < 0 ? "Left mirror housing" : "Right mirror housing",
                    new Vector3(side * 1.00f, 1.39f, 0.66f), new Vector3(0.22f, 0.14f, 0.20f), paint);
            }

            // Front lighting, grille and rear lighting.
            Part(car, PrimitiveType.Cube, "Front grille", new Vector3(0f, 0.82f, 2.145f), new Vector3(0.78f, 0.22f, 0.035f), trim);
            Part(car, PrimitiveType.Cube, "Front grille accent", new Vector3(0f, 0.82f, 2.17f), new Vector3(0.52f, 0.025f, 0.02f), chrome);
            Part(car, PrimitiveType.Cube, "Left headlight", new Vector3(-0.62f, 0.98f, 2.13f), new Vector3(0.48f, 0.18f, 0.055f), headlight);
            Part(car, PrimitiveType.Cube, "Right headlight", new Vector3(0.62f, 0.98f, 2.13f), new Vector3(0.48f, 0.18f, 0.055f), headlight);
            Part(car, PrimitiveType.Cube, "Left tail light", new Vector3(-0.62f, 1.00f, -2.13f), new Vector3(0.48f, 0.18f, 0.055f), taillight);
            Part(car, PrimitiveType.Cube, "Right tail light", new Vector3(0.62f, 1.00f, -2.13f), new Vector3(0.48f, 0.18f, 0.055f), taillight);
            Part(car, PrimitiveType.Cube, "Rear light bar", new Vector3(0f, 0.98f, -2.14f), new Vector3(0.55f, 0.045f, 0.04f), taillight);

            // Simple cockpit: dashboard, center console, seats and steering wheel.
            Part(car, PrimitiveType.Cube, "Dashboard", new Vector3(0f, 1.23f, 0.78f), new Vector3(1.38f, 0.22f, 0.38f), dashboard);
            Part(car, PrimitiveType.Cube, "Center console", new Vector3(0.12f, 0.93f, 0.05f), new Vector3(0.34f, 0.20f, 0.86f), trim);
            Part(car, PrimitiveType.Cube, "Instrument display", new Vector3(-0.36f, 1.39f, 0.79f), new Vector3(0.42f, 0.12f, 0.025f), glass);
            Part(car, PrimitiveType.Cube, "Infotainment screen", new Vector3(0.12f, 1.39f, 0.73f), new Vector3(0.32f, 0.22f, 0.035f), glass);

            Part(car, PrimitiveType.Cube, "Driver seat base", new Vector3(-0.43f, 0.98f, -0.42f), new Vector3(0.55f, 0.18f, 0.62f), seat);
            Part(car, PrimitiveType.Cube, "Driver seat back", new Vector3(-0.43f, 1.32f, -0.72f), new Vector3(0.55f, 0.65f, 0.18f), seat, new Vector3(-8f, 0f, 0f));
            Part(car, PrimitiveType.Cube, "Passenger seat base", new Vector3(0.43f, 0.98f, -0.42f), new Vector3(0.55f, 0.18f, 0.62f), seat);
            Part(car, PrimitiveType.Cube, "Passenger seat back", new Vector3(0.43f, 1.32f, -0.72f), new Vector3(0.55f, 0.65f, 0.18f), seat, new Vector3(-8f, 0f, 0f));

            // Steering wheel is a primitive disc with three simple spokes.
            Part(car, PrimitiveType.Cylinder, "Steering wheel", new Vector3(-0.43f, 1.30f, 0.48f),
                new Vector3(0.34f, 0.035f, 0.34f), trim, new Vector3(90f, 0f, 0f));
            Part(car, PrimitiveType.Cube, "Steering spoke vertical", new Vector3(-0.43f, 1.30f, 0.445f),
                new Vector3(0.035f, 0.24f, 0.035f), chrome);
            Part(car, PrimitiveType.Cube, "Steering spoke left", new Vector3(-0.52f, 1.30f, 0.445f),
                new Vector3(0.18f, 0.035f, 0.035f), chrome);
            Part(car, PrimitiveType.Cube, "Steering spoke right", new Vector3(-0.34f, 1.30f, 0.445f),
                new Vector3(0.18f, 0.035f, 0.035f), chrome);
        }

        private static GameObject Part(Transform parent, PrimitiveType type, string name,
            Vector3 position, Vector3 scale, Material material, Vector3? euler = null)
        {
            GameObject part = GameObject.CreatePrimitive(type);
            part.name = name;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = position;
            part.transform.localScale = scale;
            if (euler.HasValue) part.transform.localEulerAngles = euler.Value;
            Renderer renderer = part.GetComponent<Renderer>();
            if (renderer != null) renderer.sharedMaterial = material;
            Collider collider = part.GetComponent<Collider>();
            if (collider != null) Object.Destroy(collider);
            return part;
        }

        private static Material MakeMaterial(Color color, float metallic = 0f)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            Material material = new Material(shader);
            material.color = color;
            material.SetFloat("_Metallic", metallic);
            return material;
        }
    }
}