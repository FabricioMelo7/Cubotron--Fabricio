using System.Collections.Generic;
using UnityEngine;

public class BigCube : MonoBehaviour
{
    private GameObject m_BigCube;
    internal List<GameObject> m_SmallCubesList = new List<GameObject>();
    private Material m_trasnsparentMaterial;

    void Start()
    {
        m_BigCube = this.gameObject;
        InitBigCube();
    }

    void InitBigCube()
    {
        Shader shader = Shader.Find("Legacy Shaders/Transparent/Specular");

        m_trasnsparentMaterial = new Material(shader);
        m_trasnsparentMaterial.color = new Color(0.1f, 0.1f, 0.1f, 0.1f);

        MeshRenderer renderer = m_BigCube.GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            renderer.material = m_trasnsparentMaterial;
        }
    }
}
