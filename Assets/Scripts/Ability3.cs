using System.Collections;
using UnityEngine;

public class Ability3 : AbilityBase
{
    private GameObject m_scalingCube;
    private float m_currentScale = 0.1f;
    internal bool m_isScaling;
    private float m_maxScale = 50f;
    private bool _isCoolDown;

    public float m_despawnDelay = 3f;

    private void Start()
    {
        InitAbility();
    }

    /// <summary> 
    /// Update method called once per frame. Handles cube rotation and scaling. 
    /// </summary>
    void Update()
    {
        if (m_BigCube != null)
        {
            RotateBigCube();
            SyncSmallCubesRotation();
        }

        if (m_isScaling == true && m_scalingCube != null)
        {
            ScaleCube();
        }
    }

    /// <summary>
    /// Sets default values for the ability
    /// </summary>
    private void InitAbility()
    {
        m_maxSmallCubes = 50;
        m_rotationSpeed = 300f;
        m_fireDelay = 10f;
        m_cubesAnimationTime = 0.5f;
        m_smallCubeMinSize = 0.05f;
        m_smallCubeMaxSize = 0.9f;
    }

    /// <summary>
    /// Scales the cube over time while the fire key is held down.
    /// </summary>
    private void ScaleCube()
    {
        m_currentScale += Time.deltaTime;
        m_currentScale = Mathf.Clamp(m_currentScale, 0.2f, m_maxScale);
        m_scalingCube.transform.localScale = new Vector3(m_currentScale, m_currentScale, m_currentScale);
    }

    public override void RotateBigCube()
    {
        if (m_BigCube != null)
        {
            float rotationAmount = m_rotationSpeed * Time.deltaTime;
            m_BigCube.transform.Rotate(rotationAmount, rotationAmount, rotationAmount);
        }
    }

    /// <summary>
    /// Spawns small cubes over a set duration.
    /// </summary>
    public void SpawnCubes()
    {
        for (int i = 0; i < m_maxSmallCubes; i++)
        {
            StartCoroutine(SpawnCubeAfterDelay(i * m_cubesAnimationTime));
        }
    }

    /// <summary>
    /// Starts scaling the cube when the fire key is pressed.
    /// </summary>
    /// <param name="firePoint"> The point from which the cube is fired </param>
    public void StartScalingCube(GameObject firePoint)
    {
        if (!_isCoolDown && !m_isScaling)
        {
            m_isScaling = true;
            m_scalingCube = CubeFactory.CreateSmallCube(m_smallCubePrefab, firePoint.transform.position, Quaternion.identity);
            CubeInit(firePoint);
        }
    }

    /// <summary>
    /// Initializes the cube's parent and health, and stops it from fading
    /// </summary>
    /// <param name="firePoint"> The point from which the cube is fired </param>
    public void CubeInit(GameObject firePoint)
    {
        m_scalingCube.transform.SetParent(firePoint.transform);
        var cubeScript = m_scalingCube.GetComponent<SmallCube>();
        cubeScript.m_FadeSpeed = 0f;
    }

    /// <summary>
    /// Stops scaling and fires the cube when the fire key is released.
    /// </summary>
    /// <param name="power"> The force with which the cube is fired </param>
    /// <param name="firePoint"> The point from which the cube is fired </param>
    public void StopScalingAndFireCube(float power, GameObject firePoint)
    {
        if (m_scalingCube != null && !m_scalingCube.GetComponent<SmallCube>()._isFired)
        {
            m_isScaling = false;
            m_scalingCube.GetComponent<SmallCube>().SetFireState();
            var body = m_scalingCube.GetComponent<Rigidbody>();

            if (body != null)
            {
                body.mass = 0.5f;
                body.AddForce(firePoint.transform.forward * power, ForceMode.Impulse);
            }

            m_scalingCube.GetComponent<SmallCube>()._isFired = true;
            _isCoolDown = true;
            m_currentScale = 0.1f;
            StartCoroutine(DespawnFiredCube(m_despawnDelay));
        }
    }

    /// <summary>
    /// Coroutine to despawn the fired cube after a delay.
    /// </summary>
    /// <param name="delay"> Delay before despawning the cube. </param>
    /// <returns></returns>
    IEnumerator DespawnFiredCube(float delay)
    {
        if (m_scalingCube != null)
        {
            yield return new WaitForSeconds(delay);
            OnCubeDespawn();
            _isCoolDown = false;
        }
    }

    /// <summary>
    /// Handles the despawning of the scaling cube.
    /// </summary>
    private void OnCubeDespawn()
    {
        if (m_scalingCube != null)
        {
            Destroy(m_scalingCube);
            m_scalingCube = null;
        }
    }
}
