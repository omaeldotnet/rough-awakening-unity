using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 1.5f;
    public float sensY = 1.5f;

    public Transform orientation;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 150f); // default: mid
        float scaled = savedSensitivity / 100f;

        SetSensitivity(scaled);
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void SetSensitivity(float scaled)
    {
        sensX = scaled;
        sensY = scaled;
    }
}
