using UnityEngine;

/// <summary>
/// Adds visual street furniture to Neustadt District 01.
/// Attach this component beside NeustadtMapBuilder on the map bootstrap object.
/// These are static greybox props; traffic-light logic is intentionally not included yet.
/// </summary>
public sealed class NeustadtStreetDetails : MonoBehaviour
{
    [Header("Build options")]
    public bool buildOnStart = true;
    public bool addTrafficLights = true;
    public bool addRoadSigns = true;
    public bool addStreetNameSigns = true;
    public bool addBusStop = true;

    private Transform root;
    private Material dark;
    private Material metal;
    private Material white;
    private Material red;
    private Material amber;
    private Material green;
    private Material blue;
    private Material yellow;

    private void Start()
    {
        if (buildOnStart) Build();
    }

    [ContextMenu("Build Street Details")]
    public void Build()
    {
        if (root != null) Destroy(root.gameObject);

        root = new GameObject("Neustadt Street Details").transform;
        root.SetParent(transform, false);

        dark = MakeMaterial(new Color(0.12f, 0.14f, 0.16f));
        metal = MakeMaterial(new Color(0.32f, 0.35f, 0.37f));
        white = MakeMaterial(new Color(0.92f, 0.92f, 0.86f));
        red = MakeMaterial(new Color(0.82f, 0.08f, 0.06f));
        amber = MakeMaterial(new Color(1f, 0.58f, 0.05f));
        green = MakeMaterial(new Color(0.08f, 0.62f, 0.22f));
        blue = MakeMaterial(new Color(0.05f, 0.25f, 0.68f));
        yellow = MakeMaterial(new Color(0.95f, 0.78f, 0.12f));

        if (addTrafficLights) BuildTrafficLights();
        if (addRoadSigns) BuildRoadSigns();
        if (addStreetNameSigns) BuildStreetNames();
        if (addBusStop) BuildBusStop();
    }

    private void BuildTrafficLights()
    {
        // Four static signal assemblies around the central intersection.
        CreateSignal(new Vector3(-7.4f, 0f, -7.4f), 0f);
        CreateSignal(new Vector3(7.4f, 0f, 7.4f), 180f);
        CreateSignal(new Vector3(-7.4f, 0f, 7.4f), 90f);
        CreateSignal(new Vector3(7.4f, 0f, -7.4f), -90f);
    }

    private void CreateSignal(Vector3 position, float yaw)
    {
        Transform signal = new GameObject("Traffic Signal").transform;
        signal.SetParent(root, false);
        signal.localPosition = position;
        signal.localRotation = Quaternion.Euler(0f, yaw, 0f);

        Box(signal, "Pole", new Vector3(0f, 2.35f, 0f),
            new Vector3(0.13f, 4.7f, 0.13f), metal);
        Box(signal, "Signal Housing", new Vector3(0f, 4.25f, 0.18f),
            new Vector3(0.48f, 1.28f, 0.36f), dark);

        Sphere(signal, "Red Light", new Vector3(0f, 4.65f, 0.39f), 0.25f, red);
        Sphere(signal, "Amber Light", new Vector3(0f, 4.25f, 0.39f), 0.25f, amber);
        Sphere(signal, "Green Light", new Vector3(0f, 3.85f, 0.39f), 0.25f, green);
    }

    private void BuildRoadSigns()
    {
        // Signs are deliberately generic blockout props; final German sign artwork can replace them.
        CreateRoundSign("Speed Limit 50", new Vector3(-8.5f, 0f, 13f), 50);
        CreateRoundSign("Speed Limit 50", new Vector3(8.5f, 0f, -13f), 50);
        CreateParkingSign(new Vector3(18f, 0f, -18f));
        CreateStopSign(new Vector3(-18f, 0f, 8f));
        CreateStopSign(new Vector3(18f, 0f, -8f));
    }

    private void CreateRoundSign(string objectName, Vector3 position, int limit)
    {
        Transform sign = new GameObject(objectName).transform;
        sign.SetParent(root, false);
        sign.localPosition = position;
        Cylinder(sign, "Sign Post", new Vector3(0f, 1.45f, 0f),
            new Vector3(0.07f, 1.45f, 0.07f), metal);
        Cylinder(sign, "Sign Face", new Vector3(0f, 2.65f, 0f),
            new Vector3(0.52f, 0.08f, 0.52f), white);
        Cylinder(sign, "Red Border", new Vector3(0f, 2.65f, -0.045f),
            new Vector3(0.56f, 0.045f, 0.56f), red);
        // A small dark centre keeps the sign visibly distinct in the greybox.
        Cylinder(sign, "Number Placeholder", new Vector3(0f, 2.65f, 0.045f),
            new Vector3(0.39f, 0.025f, 0.39f), white);
    }

    private void CreateParkingSign(Vector3 position)
    {
        Transform sign = new GameObject("Parking Sign P").transform;
        sign.SetParent(root, false);
        sign.localPosition = position;
        Cylinder(sign, "Post", new Vector3(0f, 1.4f, 0f),
            new Vector3(0.07f, 1.4f, 0.07f), metal);
        Box(sign, "Blue Sign", new Vector3(0f, 2.55f, 0f),
            new Vector3(0.75f, 0.85f, 0.12f), blue);
        Box(sign, "White P Blockout", new Vector3(0f, 2.57f, -0.075f),
            new Vector3(0.24f, 0.42f, 0.025f), white);
    }

    private void CreateStopSign(Vector3 position)
    {
        Transform sign = new GameObject("Stop Sign Blockout").transform;
        sign.SetParent(root, false);
        sign.localPosition = position;
        Cylinder(sign, "Post", new Vector3(0f, 1.35f, 0f),
            new Vector3(0.07f, 1.35f, 0.07f), metal);
        Cylinder(sign, "Stop Face", new Vector3(0f, 2.55f, 0f),
            new Vector3(0.48f, 0.08f, 0.48f), red);
        // Octagonal appearance is represented by a simple red disc until final sign art is added.
    }

    private void BuildStreetNames()
    {
        CreateStreetName("HAFENSTRASSE", new Vector3(-10f, 0f, 10f), 0f);
        CreateStreetName("NEUSTAEDTER RING", new Vector3(10f, 0f, -10f), 90f);
    }

    private void CreateStreetName(string label, Vector3 position, float yaw)
    {
        Transform sign = new GameObject("Street Name - " + label).transform;
        sign.SetParent(root, false);
        sign.localPosition = position;
        sign.localRotation = Quaternion.Euler(0f, yaw, 0f);
        Box(sign, "Blue Street Plate", new Vector3(0f, 3.2f, 0f),
            new Vector3(3.4f, 0.42f, 0.12f), blue);
        // Text labels are documented here; TextMesh is used for a readable prototype label.
        GameObject textObject = new GameObject("Street Name Text");
        textObject.transform.SetParent(sign, false);
        textObject.transform.localPosition = new Vector3(0f, 3.2f, -0.075f);
        textObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        TextMesh text = textObject.AddComponent<TextMesh>();
        text.text = label;
        text.fontSize = 48;
        text.characterSize = 0.075f;
        text.anchor = TextAnchor.MiddleCenter;
        text.alignment = TextAlignment.Center;
        text.color = white.color;
    }

    private void BuildBusStop()
    {
        Transform stop = new GameObject("Neustadt Bus Stop").transform;
        stop.SetParent(root, false);
        stop.localPosition = new Vector3(13f, 0f, 23f);

        Box(stop, "Shelter Floor", new Vector3(0f, 0.12f, 0f),
            new Vector3(5.5f, 0.18f, 2.2f), metal);
        Box(stop, "Rear Panel", new Vector3(0f, 1.45f, 0.95f),
            new Vector3(5.3f, 2.7f, 0.12f), blue);
        Box(stop, "Roof", new Vector3(0f, 2.9f, 0f),
            new Vector3(5.6f, 0.16f, 2.35f), dark);
        for (int i = -1; i <= 1; i += 2)
            Box(stop, "Shelter Support", new Vector3(i * 2.55f, 1.4f, -0.9f),
                new Vector3(0.12f, 2.8f, 0.12f), metal);

        Box(stop, "Bus Stop Pole", new Vector3(-3.2f, 1.3f, -0.8f),
            new Vector3(0.12f, 2.6f, 0.12f), metal);
        Box(stop, "Bus Stop Plate", new Vector3(-3.2f, 2.45f, -0.8f),
            new Vector3(0.65f, 0.75f, 0.12f), yellow);
    }

    private GameObject Box(Transform parent, string objectName, Vector3 localPosition,
        Vector3 scale, Material material)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = objectName;
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = localPosition;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().sharedMaterial = material;
        RemoveCollider(obj);
        return obj;
    }

    private GameObject Cylinder(Transform parent, string objectName, Vector3 localPosition,
        Vector3 scale, Material material)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        obj.name = objectName;
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = localPosition;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().sharedMaterial = material;
        RemoveCollider(obj);
        return obj;
    }

    private GameObject Sphere(Transform parent, string objectName, Vector3 localPosition,
        float diameter, Material material)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.name = objectName;
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = localPosition;
        obj.transform.localScale = Vector3.one * diameter;
        obj.GetComponent<Renderer>().sharedMaterial = material;
        RemoveCollider(obj);
        return obj;
    }

    private void RemoveCollider(GameObject obj)
    {
        Collider col = obj.GetComponent<Collider>();
        if (col != null) Destroy(col);
    }

    private Material MakeMaterial(Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        Material material = new Material(shader);
        material.color = color;
        return material;
    }
}
