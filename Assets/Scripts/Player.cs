using Unity.VisualScripting;
using UnityEngine;
using CustomEventArgs = Assets.Scripts.CustomEventArgs;

public class Player : MonoBehaviour
{
    public delegate void ActionButtonEventDelegate(object sender, CustomEventArgs e);
    public event ActionButtonEventDelegate SpawnButtonEvent;
    public event ActionButtonEventDelegate UnequipButtonEvent;
    public event ActionButtonEventDelegate FireCubesEvent;

    private bool m_isCubeEquiped = false;
    internal GameObject m_Cube;
    internal AbilitiesEnum.Ability AbilityEquipped;
    internal Ability1 m_Ability1;
    internal Ability2 m_Ability2;
    internal Ability3 m_Ability3;

    private KeyCode[] abilitiesKeys;
    

    private readonly AbilitiesEnum.Ability[] abilities =
    {
        AbilitiesEnum.Ability.Ability_1,
        AbilitiesEnum.Ability.Ability_2,
        AbilitiesEnum.Ability.Ability_3,
    };

    public GameObject m_Hand;
    public float m_Power = 100;
    public KeyCode m_FireKey;
    public GameObject m_PlayerCamera;
    public GameObject m_FirePoint;
    public KeyCode ability1 = KeyCode.Alpha1;
    public KeyCode ability2 = KeyCode.Alpha2;
    public KeyCode ability3 = KeyCode.Alpha3;

    void Start()
    {
        m_FireKey = KeyCode.X; // Default key

        m_Hand.transform.SetParent(m_PlayerCamera.transform);
        m_FirePoint.transform.SetParent(m_Hand.transform);

        abilitiesKeys = new KeyCode[] { ability1, ability2, ability3 };

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
        //Handle Unequip
        if (Input.GetKeyDown(KeyCode.U) && m_isCubeEquiped)
        {
            Unequip();
            return;
        }

        //Handdle ability selection
        for (int i = 0; i < abilitiesKeys.Length; i++)
        {
            if (Input.GetKeyUp(abilitiesKeys[i]) && AbilityEquipped != abilities[i])
            {
                EquipAbility(abilities[i]);
                return;
            }
        }

        if (m_isCubeEquiped)
        {
            if (Input.GetKeyUp(m_FireKey))
            {
                if (AbilityEquipped == AbilitiesEnum.Ability.Ability_1)
                {
                    FireCubes();
                }

                else if (AbilityEquipped == AbilitiesEnum.Ability.Ability_3)
                {
                    m_Ability3.StopScalingAndFireCube(m_Power, m_FirePoint);
                }                
            }

            if(AbilityEquipped == AbilitiesEnum.Ability.Ability_3 && Input.GetKeyDown(m_FireKey))
            {               
                FireCubes();
            }

            if (Input.GetKey(m_FireKey) && AbilityEquipped == AbilitiesEnum.Ability.Ability_2)
            {
                FireCubes();
            }
        }
    }

    private void FireCubes()
    {
        FireCubesEvent?.Invoke(this, new CustomEventArgs
        {
            power = m_Power,
            obj = m_FirePoint,
            Ability = AbilityEquipped
        });
    }

    private void EquipAbility(AbilitiesEnum.Ability ability)
    {
        AbilityEquipped = ability;
        m_isCubeEquiped = true;
        SpawnCubes();
    }

    private void Unequip()
    {
        UnequipButtonEvent?.Invoke(this, null);
        m_isCubeEquiped = false;
        AbilityEquipped = AbilitiesEnum.Ability.None;
    }
}