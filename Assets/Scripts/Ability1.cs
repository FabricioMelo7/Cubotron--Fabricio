using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Ability1 : AbilityBase
{
    private void Start()
    {
        InitAbility();
    }

    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        if (m_BigCube != null)
        {
            RotateBigCube();
            SyncSmallCubesRotation();
        }
    }

    /// <summary>
    /// Sets default values for the ability
    /// </summary>
    private void InitAbility()
    {
        m_maxSmallCubes = 10;
        m_rotationSpeed = 100f;
        m_fireDelay = 0.05f;
        m_cubesAnimationTime = 0.55f;
        m_smallCubeMinSize = 0.05f;
        m_smallCubeMaxSize = 0.5f;
    }

    public void SpawnCubes()
    {
        for (int i = 0; i < m_maxSmallCubes; i++)
        {
            StartCoroutine(SpawnCubeAfterDelay(i * m_cubesAnimationTime));
        }
    }

    public override void CreateSmallCube()
    {
        if (!m_BigCube.IsUnityNull())
        {
            Vector3 randomPosition = GenerateRandomPosition(m_BigCube.GetComponent<Renderer>(), m_smallCubePrefab.transform.localScale.x) + m_BigCube.transform.position;
            GameObject smallCube = CubeFactory.CreateSmallCube(m_smallCubePrefab, randomPosition, Quaternion.identity);
            InitializeSmallCube(smallCube);
            m_bigCubeScript.m_SmallCubesList.Add(smallCube);
        }
    }

    private Vector3 GenerateRandomPosition(Renderer renderer, float smallCubeSize)
    {
        Vector3 size = renderer.bounds.size / 2 - Vector3.one * (smallCubeSize / 2);

        return new Vector3(
         Random.Range(-size.x, size.x),
         Random.Range(-size.y, size.y),
         Random.Range(-size.z, size.z));
    }

    public void FireCubes(float power, GameObject camera)
    {
        for (int i = 0; i < m_bigCubeScript.m_SmallCubesList.Count; ++i)
        {
            StartCoroutine(FireCubeWithDelay(m_bigCubeScript.m_SmallCubesList[i], i * m_fireDelay, power, camera));
        }
    }

    public IEnumerator FireCubeWithDelay(GameObject cube, float delay, float power, GameObject camera) // Delay for cubes to be lauched when firing
    {
        yield return new WaitForSeconds(delay);

        if (cube != null && cube.GetComponent<SmallCube>()._isFired == false)
        {
            var body = cube.GetComponent<Rigidbody>();

            if (body != null)
            {
                cube.GetComponent<SmallCube>()._isFired = true;
                cube.GetComponent<SmallCube>().SetFireState();

                body.AddForce(camera.transform.forward * power, ForceMode.Impulse);
            }

            StartCoroutine(SpawnNewCubesWithDelay(m_cubesAnimationTime));
        }
    }

    IEnumerator SpawnNewCubesWithDelay(float delay) // Delay for cubes to respawn after fired
    {
        for (int i = 0; i < m_maxSmallCubes; i++)
        {
            yield return new WaitForSeconds(delay * i);
            if (m_bigCubeScript.m_SmallCubesList.Count < m_maxSmallCubes)
            {
                CreateSmallCube();
            }
        }
    }
}
