using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class SmallCube : MonoBehaviour
{
    public delegate void CubeDespawnDelegate(object sender, CustomEventArgs e);
    public event CubeDespawnDelegate CubeDespawnEvent;

    internal Color m_Color;
    internal bool _isFired;
    private Material m_transparentMaterial;
    private MeshRenderer m_renderer;
    private Rigidbody m_Rigidbody;

    public float m_HealthPoints = 1f;
    public float m_CurrentHealth;
    public float m_FadeSpeed = 0.0005f;
    public float m_despawnDelay = 2f;

    void Start()
    {
        InitializeCube();
    }

    public void InitializeCube()
    {
        Shader shader = Shader.Find("Legacy Shaders/Transparent/Specular");
        m_Rigidbody = GetComponent<Rigidbody>();

        m_transparentMaterial = new Material(shader);
        m_transparentMaterial.color = m_Color;

        m_renderer = gameObject.GetComponent<MeshRenderer>();
        m_renderer.material = m_transparentMaterial;

        m_CurrentHealth = m_HealthPoints;
    }

    private void FixedUpdate()
    {
        if (m_CurrentHealth > 0f && _isFired is false)
        {
            m_CurrentHealth -= m_FadeSpeed;
            ColorFade();
        }
        else if (m_CurrentHealth <= 0f)
        {
            m_CurrentHealth = 0f;
            CubeDespawnEvent?.Invoke(this, new CustomEventArgs { obj = this.gameObject });
        }
        else if (m_CurrentHealth > 0f && _isFired is true)
        {
            StartCoroutine(DespawnAfterDelay(m_despawnDelay));
        }
    }

    void ColorFade()
    {
        float hpValue = m_CurrentHealth / m_HealthPoints;
        Color newColor = new Color(m_transparentMaterial.color.r, m_transparentMaterial.color.g, m_transparentMaterial.color.b, hpValue);
        m_transparentMaterial.color = newColor;
    }

    IEnumerator DespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_CurrentHealth = 0f;
        CubeDespawnEvent?.Invoke(this, new CustomEventArgs { obj = this.gameObject });
    }

    public void SetFireState()
    {
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.useGravity = true;
        transform.parent = null;
    }
}