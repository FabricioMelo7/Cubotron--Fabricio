using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float m_movementSpeed = 5.0f;
    private CharacterController m_characterController;

    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // left / right
        float moveVertical = Input.GetAxis("Vertical"); // forward / backwards

        Vector3 move = (transform.right * moveHorizontal) + (transform.forward * moveVertical);
        m_characterController.Move((move * m_movementSpeed) * Time.deltaTime);
    }
}