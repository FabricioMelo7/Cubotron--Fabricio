using Assets.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject m_player;
    public GameObject m_bigCubePrefab;
    public GameObject m_smallCubePrefab;

    private Player m_playerScript;
    private GameObject m_BigCube;
    private BigCube m_bigCubeScript;

    void Start()
    {
        m_playerScript = m_player.GetComponent<Player>();
        m_playerScript.SpawnButtonEvent += CubesSpawn;
        m_playerScript.UnequipButtonEvent += UnEquipCubes;
        m_playerScript.FireCubesEvent += OnFireCubes;
    }

    public void CubesSpawn(object sender, CustomEventArgs e)
    {
        if (m_BigCube != null)
        {
            Destroy(m_BigCube);
        }

        switch (e.Ability)
        {
            case AbilitiesEnum.Ability.Ability_1:
                InitAbility1(e.position);
                m_playerScript.m_Ability1.SpawnCubes();
                break;

            case AbilitiesEnum.Ability.Ability_2:
                InitAbility2(e.position);
                m_playerScript.m_Ability2.SpawnCubes();
                break;

            case AbilitiesEnum.Ability.Ability_3:
                InitAbility3(e.position);
                m_playerScript.m_Ability3.SpawnCubes();
                break;
        }
    }

    private void InitAbility1(Vector3 spawnPoint)
    {
        InitializeAbility(spawnPoint, m_playerScript.m_Ability1);
    }

    private void InitAbility2(Vector3 spawnPoint)
    {
        InitializeAbility(spawnPoint, m_playerScript.m_Ability2);
    }

    private void InitAbility3(Vector3 spawnPoint)
    {
        InitializeAbility(spawnPoint, m_playerScript.m_Ability3);
    }

    private void InitializeAbility(Vector3 spawnPoint, AbilityBase ability)
    {
        m_BigCube = Instantiate(m_bigCubePrefab, spawnPoint, Quaternion.identity);
        m_bigCubeScript = m_BigCube.GetComponent<BigCube>();
        m_playerScript.m_Cube = m_BigCube;

        ability.m_BigCube = m_BigCube;
        ability.m_bigCubeScript = m_bigCubeScript;
        ability.m_smallCubePrefab = m_smallCubePrefab;
    }

    private void UnEquipCubes(object sender, CustomEventArgs e)
    {
        Destroy(m_BigCube);
    }

    private void OnFireCubes(object sender, CustomEventArgs e)
    {
        switch (e.Ability)
        {
            case AbilitiesEnum.Ability.Ability_1:
                m_playerScript.m_Ability1.FireCubes(e.power, e.obj);
                break;

            case AbilitiesEnum.Ability.Ability_2:
                m_playerScript.m_Ability2.FireCubes(e.power, e.obj);
                break;

            case AbilitiesEnum.Ability.Ability_3:
                m_playerScript.m_Ability3.StartScalingCube(e.obj);
                break;
        }
    }
}
