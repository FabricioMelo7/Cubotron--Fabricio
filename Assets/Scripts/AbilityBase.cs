using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    internal BigCube m_bigCubeScript;
    internal GameObject m_BigCube;
    internal GameObject m_smallCubePrefab;
    internal float m_smallCubeMinSize;
    internal float m_smallCubeMaxSize;

    public int m_maxSmallCubes;
    public float m_rotationSpeed;
    public float m_fireDelay;
    public float m_cubesAnimationTime;


    /// <summary>
    /// Rotates the big cube at a constant speed.
    /// </summary>
    public virtual void RotateBigCube()
    {
        if (m_BigCube != null)
        {
            float rotationAmount = m_rotationSpeed * Time.deltaTime;
            m_BigCube.transform.Rotate(0, rotationAmount, 0);
        }
    }

    /// <summary>
    /// Synchronizes the rotation of the small cubes with the big cube.
    /// </summary>
    protected void SyncSmallCubesRotation()
    {
        if (m_BigCube == null) return;

        m_bigCubeScript.m_SmallCubesList.ForEach(x =>
        {
            if (!x.GetComponent<SmallCube>()._isFired)
            {
                x.transform.rotation = m_BigCube.transform.rotation;
            }
        });
    }

    /// <summary>
    /// Generates a random size for small cubes.
    /// </summary>
    /// <returns> A Vector3 representing the size of the small cube </returns>
    protected Vector3 GenerateRandomSize()
    {
        var size = Random.Range(m_smallCubeMinSize, m_smallCubeMaxSize);
        return new Vector3(size, size, size);
    }

    /// <summary>
    /// Initializes a small cube.
    /// </summary>
    /// <param name="cube"> The small cube to initialize </param>
    protected void InitializeSmallCube(GameObject cube)
    {
        cube.transform.parent = m_BigCube.transform;
        cube.transform.localScale = GenerateRandomSize();
        var smallCubeScript = cube.GetComponent<SmallCube>();
        smallCubeScript.CubeDespawnEvent += OnSmallCubeDespawn;
    }

    /// <summary>
    /// Handles the event when a small cube despawns.
    /// </summary>
    /// <param name="sender">SmallCube</param>
    /// <param name="e"> Event arguments containing the despawned cube </param>
    protected void OnSmallCubeDespawn(object sender, CustomEventArgs e)
    {
        GameObject cube = e.obj as GameObject;

        if (cube != null && m_bigCubeScript.m_SmallCubesList.Remove(cube))
        {
            Destroy(cube);

            if (m_bigCubeScript.m_SmallCubesList.Count < m_maxSmallCubes)
            {
                StartCoroutine(SpawnCubeAfterDelay(m_bigCubeScript.m_SmallCubesList.Count * 0.2f));
            }
        }
    }

    /// <summary>
    /// Coroutine to spawn cubes with a delay
    /// </summary>
    /// <param name="delay"> Delay before spawning the next cube </param>
    /// <returns></returns>
    protected IEnumerator SpawnCubeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (m_bigCubeScript.m_SmallCubesList.Count < m_maxSmallCubes)
        {
            CreateSmallCube();
        }
    }

    /// <summary>
    /// Creates a small cube and initializes it.
    /// </summary>
    public virtual void CreateSmallCube()
    {
        if (m_BigCube != null)
        {
            GameObject smallCube = CubeFactory.CreateSmallCube(m_smallCubePrefab, m_BigCube.transform.position, Quaternion.identity);
            InitializeSmallCube(smallCube);
            m_bigCubeScript.m_SmallCubesList.Add(smallCube);
        }
    }
}
