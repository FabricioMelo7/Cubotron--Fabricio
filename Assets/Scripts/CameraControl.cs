using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_mouseSensitivity = 50f;
    public Transform m_playerBody;

    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        var x = Input.GetAxis("Mouse X");

        float mouseX = (Input.GetAxis("Mouse X") * m_mouseSensitivity) * Time.deltaTime;
        float mouseY = (Input.GetAxis("Mouse Y") * m_mouseSensitivity) * Time.deltaTime;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        m_playerBody.Rotate(Vector3.up * mouseX);
    }
}
