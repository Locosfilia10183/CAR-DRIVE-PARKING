using UnityEngine;

namespace CarDriveParking
{
    /// <summary>
    /// Adds recognizable facade details to the first Neustadt greybox district.
    /// Attach beside NeustadtMapBuilder. Decorative only; no gameplay logic.
    /// </summary>
    public sealed class NeustadtBuildingDetails : MonoBehaviour
    {
        [SerializeField] private bool buildOnStart = true;
        [SerializeField] private bool apartmentBalconies = true;
        [SerializeField] private bool shopFronts = true;
        [SerializeField] private bool rooftopDetails = true;

        private Transform root;
        private Material trim;
        private Material glass;
        private Material dark;
        private Material awning;
        private Material concrete;
        private Material signBlue;

        private void Start()
        {
            if (buildOnStart) Build();
        }

        [ContextMenu("Build Neustadt Building Details")]
        public void Build()
        {
            if (root != null) Destroy(root.gameObject);
            root = new GameObject("Neustadt Building Facade Details").transform;
            root.SetParent(transform, false);

            trim = Mat(new Color(0.83f, 0.81f, 0.75f));
            glass = Mat(new Color(0.10f, 0.30f, 0.39f));
            dark = Mat(new Color(0.12f, 0.14f, 0.16f));
            awning = Mat(new Color(0.72f, 0.16f, 0.12f));
            concrete = Mat(new Color(0.48f, 0.49f, 0.47f));
            signBlue = Mat(new Color(0.05f, 0.20f, 0.42f));

            if (rooftopDetails)
            {
                AddRoofTrim("Corner Cafe", new Vector3(-28f, 13f, 27f), new Vector3(18f, 0.45f, 18f));
                AddRoofTrim("Apartments A", new Vector3(-55f, 21f, 29f), new Vector3(22f, 0.55f, 20f));
                AddRoofTrim("City Office", new Vector3(29f, 25f, 29f), new Vector3(20f, 0.55f, 19f));
                AddRoofTrim("Apartments B", new Vector3(57f, 18f, 30f), new Vector3(23f, 0.5f, 21f));
                AddRoofTrim("Autohaus", new Vector3(-29f, 11f, -29f), new Vector3(23f, 0.4f, 18f));
                AddRoofTrim("Workshop", new Vector3(57f, 10f, -29f), new Vector3(22f, 0.4f, 19f));
                AddRoofTrim("Residential C", new Vector3(-58f, 17f, -29f), new Vector3(22f, 0.45f, 21f));
                AddRoofTrim("Residential D", new Vector3(85f, 15f, -30f), new Vector3(18f, 0.45f, 21f));
            }

            if (apartmentBalconies)
            {
                AddBalconies("Apartment A", -55f, 29f, 22f, 21f, 3);
                AddBalconies("Apartment B", 57f, 30f, 23f, 18f, 2);
                AddBalconies("Residential C", -58f, -29f, 22f, 17f, 2);
                AddBalconies("Residential D", 85f, -30f, 18f, 15f, 2);
            }

            if (shopFronts)
            {
                AddCafeFront();
                AddDealershipFront();
                AddWorkshopDoors();
                AddOfficeEntrance();
            }
        }

        private void AddRoofTrim(string label, Vector3 center, Vector3 size)
        {
            Box(label + " roof edge front", new Vector3(center.x, center.y, center.z - size.z * 0.5f),
                new Vector3(size.x + 0.35f, size.y, 0.35f), trim);
            Box(label + " roof edge rear", new Vector3(center.x, center.y, center.z + size.z * 0.5f),
                new Vector3(size.x + 0.35f, size.y, 0.35f), trim);
            Box(label + " roof edge left", new Vector3(center.x - size.x * 0.5f, center.y, center.z),
                new Vector3(0.35f, size.y, size.z), trim);
            Box(label + " roof edge right", new Vector3(center.x + size.x * 0.5f, center.y, center.z),
                new Vector3(0.35f, size.y, size.z), trim);
        }

        private void AddBalconies(string label, float x, float z, float width, float height, int floors)
        {
            for (int floor = 1; floor <= floors; floor++)
            {
                float y = 2.7f + floor * 4.2f;
                for (int side = -1; side <= 1; side += 2)
                {
                    float bx = x + side * width * 0.24f;
                    Box(label + " balcony slab", new Vector3(bx, y, z - 10.7f),
                        new Vector3(width * 0.34f, 0.18f, 1.35f), concrete);
                    Box(label + " balcony front rail", new Vector3(bx, y + 0.55f, z - 11.3f),
                        new Vector3(width * 0.34f, 0.08f, 0.08f), dark);
                    for (int post = -1; post <= 1; post++)
                        Box(label + " balcony rail post", new Vector3(bx + post * width * 0.12f, y + 0.28f, z - 11.3f),
                            new Vector3(0.07f, 0.55f, 0.07f), dark);
                }
            }
        }

        private void AddCafeFront()
        {
            float x = -28f;
            float z = 27f - 9.15f;
            Box("Cafe display glazing", new Vector3(x, 2.1f, z - 0.08f),
                new Vector3(8.8f, 2.4f, 0.12f), glass);
            Box("Cafe red awning", new Vector3(x, 3.55f, z - 0.45f),
                new Vector3(10.5f, 0.28f, 1.15f), awning);
            Box("Cafe sign board", new Vector3(x, 5.0f, z - 0.1f),
                new Vector3(7.5f, 0.85f, 0.16f), signBlue);
            TextMesh label = Text("CAFE", new Vector3(x, 5.0f, z - 0.205f), 0.12f);
            label.color = Color.white;
        }

        private void AddDealershipFront()
        {
            float x = -29f;
            float z = -29f + 9.15f;
            Box("Autohaus showroom glazing", new Vector3(x, 3.2f, z + 0.08f),
                new Vector3(13f, 5.4f, 0.12f), glass);
            Box("Autohaus fascia", new Vector3(x, 6.7f, z + 0.12f),
                new Vector3(17f, 0.8f, 0.22f), signBlue);
            Text("AUTOHAUS", new Vector3(x, 6.7f, z + 0.0f), 0.095f).color = Color.white;
            Box("Showroom lower trim", new Vector3(x, 0.45f, z + 0.12f),
                new Vector3(17f, 0.35f, 0.25f), trim);
        }

        private void AddWorkshopDoors()
        {
            float x = 57f;
            float z = -29f - 9.65f;
            for (int bay = -1; bay <= 1; bay++)
            {
                float bx = x + bay * 6.2f;
                Box("Workshop roller door", new Vector3(bx, 3.0f, z - 0.08f),
                    new Vector3(5.3f, 5.8f, 0.18f), dark);
                for (int line = 0; line < 7; line++)
                    Box("Roller door slat", new Vector3(bx, 0.7f + line * 0.72f, z - 0.19f),
                        new Vector3(5.15f, 0.045f, 0.035f), metalMaterial);
            }
        }

        private Material metalMaterial
        {
            get { return concrete; }
        }

        private void AddOfficeEntrance()
        {
            float x = 29f;
            float z = 29f - 9.65f;
            Box("City Office glass entrance", new Vector3(x, 1.6f, z - 0.09f),
                new Vector3(3.2f, 3.2f, 0.14f), glass);
            Box("City Office canopy", new Vector3(x, 3.55f, z - 0.8f),
                new Vector3(6.5f, 0.22f, 1.5f), dark);
        }

        private TextMesh Text(string value, Vector3 position, float characterSize)
        {
            GameObject obj = new GameObject(value + " Sign Text");
            obj.transform.SetParent(root, false);
            obj.transform.position = position;
            TextMesh mesh = obj.AddComponent<TextMesh>();
            mesh.text = value;
            mesh.fontSize = 64;
            mesh.characterSize = characterSize;
            mesh.anchor = TextAnchor.MiddleCenter;
            mesh.alignment = TextAlignment.Center;
            return mesh;
        }

        private GameObject Box(string label, Vector3 position, Vector3 size, Material material)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = label;
            obj.transform.SetParent(root, false);
            obj.transform.position = position;
            obj.transform.localScale = size;
            obj.GetComponent<Renderer>().sharedMaterial = material;
            Collider collider = obj.GetComponent<Collider>();
            if (collider != null) Destroy(collider);
            return obj;
        }

        private Material Mat(Color color)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            Material material = new Material(shader);
            material.color = color;
            return material;
        }
    }
}