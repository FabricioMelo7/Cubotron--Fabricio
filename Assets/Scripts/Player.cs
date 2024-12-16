using Assets.Scripts;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void ActionButtonEventDelegate(object sender, CustomEventArgs e);
    public event ActionButtonEventDelegate SpawnButtonEvent;
    public event ActionButtonEventDelegate UnequipButtonEvent;
    public event ActionButtonEventDelegate FireCubesEvent;

    private bool m_isCubeEquiped = false;
    internal GameObject m_Cube;
    internal Enum AbilityEquipped;
    internal Ability1 m_Ability1;
    internal Ability2 m_Ability2;
    internal Ability3 m_Ability3;

    public GameObject m_Hand;
    public float m_Power = 100;
    public KeyCode m_FireKey;
    public GameObject m_PlayerCamera;
    public GameObject m_FirePoint;

    void Start()
    {
        m_FireKey = KeyCode.X; // Default key

        m_Hand.transform.SetParent(m_PlayerCamera.transform);
        m_FirePoint.transform.SetParent(m_Hand.transform);

        m_Ability1 = GetComponent<Ability1>();
        m_Ability2 = GetComponent<Ability2>();
        m_Ability3 = GetComponent<Ability3>();
    }

    void Update()
    {
        HandleKeyboardInput();
    }

    private void FixedUpdate()
    {
        if (m_Cube != null)
        {
            m_Cube.transform.position = HandCameraSync();
        }
    }

    public Vector3 CubeSpawnPoint()
    {
        Vector3 spawnPoint = new Vector3
        {
            x = m_Hand.transform.position.x,
            y = m_Hand.transform.position.y + 0.3f,
            z = m_Hand.transform.position.z
        };

        return spawnPoint;
    }

    private Vector3 HandCameraSync()
    {
        return m_Hand.transform.position + new Vector3(0f, 0.3f, 0f);
    }

    private void SpawnCubes()
    {
        SpawnButtonEvent?.Invoke(this, new CustomEventArgs { position = CubeSpawnPoint(), Ability = AbilityEquipped });
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.U) && m_isCubeEquiped == true)
        {
            UnequipButtonEvent?.Invoke(this, null);
            m_isCubeEquiped = false;
            AbilityEquipped = AbilitiesEnum.Ability.None;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha1) && AbilityEquipped is not AbilitiesEnum.Ability.Ability_1)
        {
            AbilityEquipped = AbilitiesEnum.Ability.Ability_1;
            m_isCubeEquiped = true;
            SpawnCubes();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2) && AbilityEquipped is not AbilitiesEnum.Ability.Ability_2)
        {
            AbilityEquipped = AbilitiesEnum.Ability.Ability_2;
            m_isCubeEquiped = true;
            SpawnCubes();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3) && AbilityEquipped is not AbilitiesEnum.Ability.Ability_3)
        {
            AbilityEquipped = AbilitiesEnum.Ability.Ability_3;
            m_isCubeEquiped = true;
            SpawnCubes();
        }

        if (Input.GetKeyUp(m_FireKey) && m_isCubeEquiped == true && AbilityEquipped.Equals(AbilitiesEnum.Ability.Ability_1))
        {
            FireCubes();
        }

        if (Input.GetKey(m_FireKey) && m_isCubeEquiped == true && AbilityEquipped.Equals(AbilitiesEnum.Ability.Ability_2))
        {
            FireCubes();
        }

        if (Input.GetKeyDown(m_FireKey) && m_isCubeEquiped == true && AbilityEquipped.Equals(AbilitiesEnum.Ability.Ability_3))
        {
            FireCubes();
        }

        if (Input.GetKeyUp(m_FireKey) && m_isCubeEquiped == true && AbilityEquipped.Equals(AbilitiesEnum.Ability.Ability_3))
        {
            m_Ability3.StopScalingAndFireCube(m_Power, m_FirePoint);
        }
    }

    private void FireCubes()
    {
        switch (AbilityEquipped)
        {
            case AbilitiesEnum.Ability.Ability_1:
                FireCubesEvent?.Invoke(this, new CustomEventArgs { power = m_Power, obj = m_PlayerCamera, Ability = AbilityEquipped });
                break;

            case AbilitiesEnum.Ability.Ability_2:
                FireCubesEvent?.Invoke(this, new CustomEventArgs { power = m_Power, obj = m_FirePoint, Ability = AbilityEquipped });
                break;

            case AbilitiesEnum.Ability.Ability_3:
                FireCubesEvent?.Invoke(this, new CustomEventArgs { power = m_Power, obj = m_FirePoint, Ability = AbilityEquipped });
                break;
        }
    }
}