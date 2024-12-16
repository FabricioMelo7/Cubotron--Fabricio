using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ability2 : AbilityBase
{
    private Queue<GameObject> FiredCubes = new Queue<GameObject>();

    private void Start()
    {
        InitAbility();
    }

    void Update()
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
        m_maxSmallCubes = 20;
        m_rotationSpeed = 200f;
        m_fireDelay = 1f;
        m_cubesAnimationTime = 0.1f;
        m_smallCubeMinSize = 0.05f;
        m_smallCubeMaxSize = 0.8f;
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
            GameObject smallCube = CubeFactory.CreateSmallCube(m_smallCubePrefab, m_BigCube.transform.position, Quaternion.identity);
            InitializeSmallCube(smallCube);
            m_bigCubeScript.m_SmallCubesList.Add(smallCube);
        }
    }

    public override void RotateBigCube()
    {
        if (m_BigCube != null)
        {
            float rotationAmount = m_rotationSpeed * Time.deltaTime;
            m_BigCube.transform.Rotate(rotationAmount, 0, 0);
        }
    }

    public void FireCubes(float power, GameObject firePoint)
    {
        StartCoroutine(FireCubeWithDelay(m_fireDelay, power, firePoint));
    }

    public IEnumerator FireCubeWithDelay(float delay, float power, GameObject firePoint)
    {
        yield return new WaitForSeconds(delay);

        if (m_BigCube != null)
        {
            GameObject cube = Ability2SmallCube(firePoint);

            if (cube != null)
            {
                var body = cube.GetComponent<Rigidbody>();

                if (body != null)
                {
                    cube.GetComponent<SmallCube>()._isFired = true;

                    body.AddForce(firePoint.transform.forward * power, ForceMode.Impulse);
                }
            }
        }

        StartCoroutine(DespawnFiredCubes(1f));
    }

    private GameObject Ability2SmallCube(GameObject firePoint)
    {
        Vector3 fireP = new Vector3(firePoint.transform.position.x, firePoint.transform.position.y * 1f, firePoint.transform.position.z);
        GameObject cube = CubeFactory.CreateSmallCube(m_smallCubePrefab, fireP, Quaternion.identity);
        InitializeSmallCube(cube);
        cube.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        cube.GetComponent<SmallCube>().SetFireState();
        FiredCubes.Enqueue(cube);

        return cube;
    }

    IEnumerator DespawnFiredCubes(float delay)
    {
        if (FiredCubes.Count > 0)
        {
            yield return new WaitForSeconds(delay);

            var cube = FiredCubes.Count == 0 ? null : FiredCubes.Dequeue();

            if (cube != null)
            {
                Destroy(cube);
            }
        }
    }
}
