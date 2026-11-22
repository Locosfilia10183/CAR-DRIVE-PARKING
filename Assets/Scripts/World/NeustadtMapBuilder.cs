using UnityEngine;

namespace CarDriveParking
{
    /// <summary>
    /// Generates the first small Neustadt city blockout from Unity primitives.
    /// Attach to an empty GameObject in a new scene and press Play.
    /// This is a greybox layout for scale and driving tests, not final art.
    /// </summary>
    public class NeustadtMapBuilder : MonoBehaviour
    {
        [SerializeField] private bool buildOnPlay = true;
        [SerializeField] private bool createStreetFurniture = true;

        private Transform mapRoot;
        private Material groundMaterial;
        private Material roadMaterial;
        private Material sidewalkMaterial;
        private Material lineMaterial;
        private Material[] buildingMaterials;
        private Material windowMaterial;
        private Material grassMaterial;
        private Material treeMaterial;
        private Material lampMaterial;

        private void Start()
        {
            if (buildOnPlay)
                BuildMap();
        }

        [ContextMenu("Build Neustadt Blockout")]
        public void BuildMap()
        {
            if (mapRoot != null)
                Destroy(mapRoot.gameObject);

            mapRoot = new GameObject("NEUSTADT - First District").transform;
            mapRoot.SetParent(transform, false);
            CreateMaterials();
            CreateGroundAndRoads();
            CreateIntersectionMarkings();
            CreateParkingLot();
            CreateBuildings();
            CreateSmallPark();
            CreatePreviewLightingAndCamera();

            if (createStreetFurniture)
            {
                CreateStreetLights();
                CreateTrees();
            }

            Debug.Log("NEUSTADT blockout created: central avenue, cross street, sidewalks, parking, buildings and park.", this);
        }

        private void CreatePreviewLightingAndCamera()
        {
            // A simple overview camera makes the generated district visible in a brand-new empty scene.
            Camera existingCamera = Camera.main;
            if (existingCamera != null)
                existingCamera.enabled = false;

            GameObject cameraObject = new GameObject("Neustadt Overview Camera");
            cameraObject.transform.SetParent(mapRoot, false);
            cameraObject.transform.position = new Vector3(118f, 145f, -145f);
            cameraObject.transform.LookAt(new Vector3(0f, 0f, 0f));
            Camera overview = cameraObject.AddComponent<Camera>();
            overview.tag = "MainCamera";
            overview.fieldOfView = 55f;
            overview.nearClipPlane = 0.3f;
            overview.farClipPlane = 500f;
            overview.backgroundColor = new Color(0.38f, 0.52f, 0.62f);
            overview.clearFlags = CameraClearFlags.SolidColor;

            GameObject sunObject = new GameObject("Neustadt Sun");
            sunObject.transform.SetParent(mapRoot, false);
            sunObject.transform.rotation = Quaternion.Euler(48f, -32f, 0f);
            Light sun = sunObject.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.intensity = 1.15f;
            sun.color = new Color(1f, 0.94f, 0.82f);
            RenderSettings.ambientLight = new Color(0.48f, 0.52f, 0.58f);
        }

        private void CreateMaterials()
        {
            groundMaterial = MakeMaterial(new Color(0.20f, 0.25f, 0.20f));
            roadMaterial = MakeMaterial(new Color(0.105f, 0.12f, 0.14f));
            sidewalkMaterial = MakeMaterial(new Color(0.42f, 0.44f, 0.44f));
            lineMaterial = MakeMaterial(new Color(0.88f, 0.86f, 0.73f));
            windowMaterial = MakeMaterial(new Color(0.12f, 0.28f, 0.36f), 0.15f);
            grassMaterial = MakeMaterial(new Color(0.12f, 0.28f, 0.13f));
            treeMaterial = MakeMaterial(new Color(0.10f, 0.25f, 0.11f));
            lampMaterial = MakeMaterial(new Color(0.16f, 0.18f, 0.20f), 0.45f);

            buildingMaterials = new[]
            {
                MakeMaterial(new Color(0.48f, 0.45f, 0.40f)),
                MakeMaterial(new Color(0.38f, 0.43f, 0.48f)),
                MakeMaterial(new Color(0.58f, 0.55f, 0.48f)),
                MakeMaterial(new Color(0.32f, 0.37f, 0.39f)),
                MakeMaterial(new Color(0.52f, 0.40f, 0.34f))
            };
        }

        private void CreateGroundAndRoads()
        {
            CreateBox("Terrain", new Vector3(0f, -0.18f, 0f), new Vector3(260f, 0.3f, 260f), groundMaterial, true);

            // Main north-south avenue and the east-west cross street.
            CreateBox("Hauptallee - Asphalt", new Vector3(0f, 0.005f, 0f), new Vector3(13f, 0.08f, 210f), roadMaterial, false);
            CreateBox("Ringstrasse - Asphalt", new Vector3(0f, 0.006f, 0f), new Vector3(210f, 0.08f, 13f), roadMaterial, false);

            // Continuous sidewalks along the main avenue.
            for (int side = -1; side <= 1; side += 2)
            {
                CreateBox("Hauptallee sidewalk", new Vector3(side * 8.2f, 0.10f, 0f), new Vector3(3.2f, 0.22f, 210f), sidewalkMaterial, true);
                CreateBox("Ringstrasse sidewalk", new Vector3(0f, 0.10f, side * 8.2f), new Vector3(210f, 0.22f, 3.2f), sidewalkMaterial, true);
            }

            // Lane divider dashes on both roads, stopping at the junction.
            for (int z = -96; z <= 96; z += 12)
            {
                if (Mathf.Abs(z) < 12) continue;
                CreateBox("Avenue lane dash", new Vector3(0f, 0.058f, z), new Vector3(0.16f, 0.018f, 5.5f), lineMaterial, false);
            }

            for (int x = -96; x <= 96; x += 12)
            {
                if (Mathf.Abs(x) < 12) continue;
                CreateBox("Ringstrasse lane dash", new Vector3(x, 0.059f, 0f), new Vector3(5.5f, 0.018f, 0.16f), lineMaterial, false);
            }

            // Simple stop lines before the central intersection.
            CreateBox("Stop line north", new Vector3(0f, 0.062f, 10.5f), new Vector3(12.5f, 0.02f, 0.22f), lineMaterial, false);
            CreateBox("Stop line south", new Vector3(0f, 0.062f, -10.5f), new Vector3(12.5f, 0.02f, 0.22f), lineMaterial, false);
            CreateBox("Stop line east", new Vector3(10.5f, 0.062f, 0f), new Vector3(0.22f, 0.02f, 12.5f), lineMaterial, false);
            CreateBox("Stop line west", new Vector3(-10.5f, 0.062f, 0f), new Vector3(0.22f, 0.02f, 12.5f), lineMaterial, false);
        }

        private void CreateIntersectionMarkings()
        {
            // Four compact zebra crossings around the central junction.
            for (int i = -4; i <= 4; i++)
            {
                float offset = i * 0.85f;
                CreateBox("Crosswalk north", new Vector3(offset, 0.065f, 9.1f), new Vector3(0.48f, 0.018f, 2.1f), lineMaterial, false);
                CreateBox("Crosswalk south", new Vector3(offset, 0.065f, -9.1f), new Vector3(0.48f, 0.018f, 2.1f), lineMaterial, false);
                CreateBox("Crosswalk east", new Vector3(9.1f, 0.065f, offset), new Vector3(2.1f, 0.018f, 0.48f), lineMaterial, false);
                CreateBox("Crosswalk west", new Vector3(-9.1f, 0.065f, offset), new Vector3(2.1f, 0.018f, 0.48f), lineMaterial, false);
            }
        }

        private void CreateParkingLot()
        {
            // Public parking area in the south-east city block.
            CreateBox("Public parking asphalt", new Vector3(29f, 0.012f, -27f), new Vector3(34f, 0.08f, 24f), roadMaterial, false);

            for (int row = 0; row < 2; row++)
            {
                float z = -20f - row * 13f;
                for (int bay = 0; bay < 7; bay++)
                {
                    float x = 17f + bay * 4f;
                    CreateBox("Parking bay side line", new Vector3(x, 0.064f, z), new Vector3(0.10f, 0.02f, 5.2f), lineMaterial, false);
                }
                CreateBox("Parking bay back line", new Vector3(29f, 0.064f, z + 2.6f), new Vector3(28f, 0.02f, 0.10f), lineMaterial, false);
            }

            CreateBox("Parking entrance island", new Vector3(29f, 0.16f, -13.6f), new Vector3(34f, 0.28f, 1.1f), grassMaterial, true);
            CreateBox("Parking entrance curb", new Vector3(29f, 0.12f, -14.2f), new Vector3(34f, 0.18f, 0.18f), sidewalkMaterial, true);
        }

        private void CreateBuildings()
        {
            // Downtown-facing buildings north-west and north-east.
            CreateBuilding("Corner Cafe", new Vector3(-28f, 0f, 27f), new Vector3(18f, 13f, 18f), 0);
            CreateBuilding("Neustadt Apartments A", new Vector3(-55f, 0f, 29f), new Vector3(22f, 21f, 20f), 1);
            CreateBuilding("City Office", new Vector3(29f, 0f, 29f), new Vector3(20f, 25f, 19f), 2);
            CreateBuilding("Neustadt Apartments B", new Vector3(57f, 0f, 30f), new Vector3(23f, 18f, 21f), 3);

            // South side: shops, service building and low-rise housing.
            CreateBuilding("Autohaus Neustadt", new Vector3(-29f, 0f, -29f), new Vector3(23f, 11f, 18f), 3);
            CreateBuilding("Residential Block C", new Vector3(-58f, 0f, -29f), new Vector3(22f, 17f, 21f), 0);
            CreateBuilding("Workshop", new Vector3(57f, 0f, -29f), new Vector3(22f, 10f, 19f), 4);
            CreateBuilding("Residential Block D", new Vector3(85f, 0f, -30f), new Vector3(18f, 15f, 21f), 2);

            // A few additional blocks give the first district a believable edge.
            CreateBuilding("Office Annex", new Vector3(-87f, 0f, 28f), new Vector3(18f, 16f, 20f), 4);
            CreateBuilding("Storage Hall", new Vector3(88f, 0f, 29f), new Vector3(19f, 12f, 22f), 1);
        }

        private void CreateBuilding(string buildingName, Vector3 position, Vector3 size, int materialIndex)
        {
            float height = size.y;
            Material facade = buildingMaterials[Mathf.Clamp(materialIndex, 0, buildingMaterials.Length - 1)];
            CreateBox(buildingName, new Vector3(position.x, height * 0.5f, position.z), size, facade, true);

            // Repeated dark window panels on the street-facing facade.
            int columns = Mathf.Max(2, Mathf.FloorToInt(size.x / 4f));
            int rows = Mathf.Max(2, Mathf.FloorToInt(height / 4f));
            float spacingX = size.x / (columns + 1);
            float spacingY = height / (rows + 1);

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    float x = position.x - size.x * 0.5f + spacingX * (column + 1);
                    float y = 1.7f + spacingY * row;
                    float frontZ = position.z - size.z * 0.5f - 0.035f;
                    CreateBox(buildingName + " window", new Vector3(x, y, frontZ),
                        new Vector3(1.35f, 1.45f, 0.06f), windowMaterial, false);
                }
            }

            // Simple entrance panel on the facade.
            CreateBox(buildingName + " entrance", new Vector3(position.x, 1.1f, position.z - size.z * 0.5f - 0.045f),
                new Vector3(1.8f, 2.2f, 0.08f), lampMaterial, false);
        }

        private void CreateSmallPark()
        {
            CreateBox("Neighbourhood park grass", new Vector3(-31f, 0.015f, 69f), new Vector3(46f, 0.08f, 30f), grassMaterial, false);
            CreateBox("Park path", new Vector3(-31f, 0.07f, 69f), new Vector3(4f, 0.06f, 30f), sidewalkMaterial, false);
            CreateBox("Park path cross", new Vector3(-31f, 0.075f, 69f), new Vector3(46f, 0.06f, 3f), sidewalkMaterial, false);
            CreateBox("Park pond", new Vector3(-43f, 0.09f, 75f), new Vector3(9f, 0.08f, 6f), MakeMaterial(new Color(0.08f, 0.27f, 0.38f)), false);
        }

        private void CreateStreetLights()
        {
            for (int z = -84; z <= 84; z += 28)
            {
                if (Mathf.Abs(z) < 18) continue;
                CreateLamp(new Vector3(-10.5f, 0f, z));
                CreateLamp(new Vector3(10.5f, 0f, z));
            }

            for (int x = -84; x <= 84; x += 28)
            {
                if (Mathf.Abs(x) < 18) continue;
                CreateLamp(new Vector3(x, 0f, -10.5f));
                CreateLamp(new Vector3(x, 0f, 10.5f));
            }
        }

        private void CreateLamp(Vector3 position)
        {
            CreateBox("Street lamp pole", position + new Vector3(0f, 3.2f, 0f), new Vector3(0.16f, 6.4f, 0.16f), lampMaterial, true);
            CreateBox("Street lamp arm", position + new Vector3(0.65f, 6.2f, 0f), new Vector3(1.4f, 0.13f, 0.13f), lampMaterial, true);
            CreateBox("Street lamp head", position + new Vector3(1.25f, 6.05f, 0f), new Vector3(0.55f, 0.16f, 0.35f), lineMaterial, false);
        }

        private void CreateTrees()
        {
            Vector3[] positions =
            {
                new Vector3(-48f, 0f, 61f), new Vector3(-15f, 0f, 61f),
                new Vector3(-48f, 0f, 78f), new Vector3(-15f, 0f, 78f),
                new Vector3(-12f, 0f, 38f), new Vector3(12f, 0f, 38f),
                new Vector3(-12f, 0f, -39f), new Vector3(12f, 0f, -39f),
                new Vector3(-77f, 0f, 3f), new Vector3(77f, 0f, 3f)
            };

            foreach (Vector3 position in positions)
            {
                CreateBox("Tree trunk", position + new Vector3(0f, 1.2f, 0f), new Vector3(0.45f, 2.4f, 0.45f), lampMaterial, true);
                GameObject crown = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                crown.name = "Tree crown";
                crown.transform.SetParent(mapRoot, false);
                crown.transform.position = position + new Vector3(0f, 3.1f, 0f);
                crown.transform.localScale = new Vector3(3.2f, 3.2f, 3.2f);
                crown.GetComponent<Renderer>().sharedMaterial = treeMaterial;
                // Keep the crown collider off so the primitive canopy does not block cars.
                Collider collider = crown.GetComponent<Collider>();
                if (collider != null) Destroy(collider);
            }
        }

        private GameObject CreateBox(string objectName, Vector3 position, Vector3 size, Material material, bool keepCollider)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = objectName;
            obj.transform.SetParent(mapRoot, false);
            obj.transform.position = position;
            obj.transform.localScale = size;
            obj.GetComponent<Renderer>().sharedMaterial = material;
            if (!keepCollider)
            {
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null) Destroy(collider);
            }
            return obj;
        }

        private Material MakeMaterial(Color color, float metallic = 0f)
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
