using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 50;
    public float mouseSensitivity = 2f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Update()
    {
        speed += Input.mouseScrollDelta.y*Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, 100);

        if (Input.GetKeyDown(KeyCode.R))
        {
            speed = 50;
            transform.position = Vector3.zero;
            rotationX = 0f;
            rotationY = 0f;
            transform.rotation = Quaternion.identity;
        }

        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.E))
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.Q))
            transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (Input.GetMouseButtonDown(1))
            Cursor.lockState = CursorLockMode.Locked;

        if (Input.GetMouseButton(1))
        {
            rotationY += Input.GetAxis("Mouse X") * mouseSensitivity;
            rotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }

        if (Input.GetMouseButtonUp(1))
            Cursor.lockState = CursorLockMode.None;

        if (Input.GetMouseButton(2))
        {
            rotationX = 0f;
            rotationY = 0f;
            transform.rotation = Quaternion.identity;
        }
    }
}
