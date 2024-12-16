using UnityEngine;

public static class CubeFactory
{
    public static GameObject CreateSmallCube(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject smallCube = GameObject.Instantiate(prefab, position, rotation);
        InitializeSmallCube(smallCube);
        return smallCube;
    }

    private static void InitializeSmallCube(GameObject cube)
    {
        cube.transform.localScale = GenerateRandomSize();
        var smallCubeScript = cube.GetComponent<SmallCube>();
        smallCubeScript.m_HealthPoints = GenerateRandomHealth();
        smallCubeScript.m_Color = GenerateRandomColor();
        smallCubeScript.InitializeCube();

        Rigidbody rb = cube.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.angularDrag = 0f;
            rb.interpolation = RigidbodyInterpolation.Extrapolate;
        }
    }

    private static float GenerateRandomHealth()
    {
        return Random.Range(0f, 1f);
    }

    private static Color GenerateRandomColor()
    {
        return new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            1.0f
        );
    }

    private static Vector3 GenerateRandomSize()
    {
        var size = Random.Range(0.02f, 0.15f);
        return new Vector3(size, size, size);
    }
}