using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Transform doorVisual; // <-- Reference to the child that actually moves

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    public float openAngle = 90f;
    public float smoothSpeed = 3f;

    private void Start()
    {
        if (doorVisual == null)
            doorVisual = transform; // fallback if you forget to assign

        closedRotation = doorVisual.rotation;
        openRotation = Quaternion.Euler(doorVisual.eulerAngles + new Vector3(0, openAngle, 0));
    }

    private void Update()
    {
        if (isOpen)
        {
            doorVisual.rotation = Quaternion.Slerp(doorVisual.rotation, openRotation, Time.deltaTime * smoothSpeed);
        }
        else
        {
            doorVisual.rotation = Quaternion.Slerp(doorVisual.rotation, closedRotation, Time.deltaTime * smoothSpeed);
        }
    }

    public void Interact()
    {
        isOpen = !isOpen;
    }
}
