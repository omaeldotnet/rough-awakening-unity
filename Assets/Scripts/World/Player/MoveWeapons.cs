using UnityEngine;

public class MoveWeapon : MonoBehaviour
{
    public Transform povPosition;
    public Transform povRotation;

    
    private static float xOffset = -0.2f; //left or right
    private static float yOffset = 0.2f; //up or down
    private static float zOffset = 0.6f; //infront or back
    

    [SerializeField] public Vector3 offset = new Vector3(xOffset,yOffset,zOffset);

    private void Update()
    {
        Vector3 offsetPosition = povPosition.position
                               + povRotation.forward * offset.z
                               + povRotation.right * offset.x
                               + povRotation.up * offset.y;
        

        //updates position of weapon based of player
        transform.position = offsetPosition;

        transform.rotation = Quaternion.Euler(0, povRotation.eulerAngles.y, 0);

       
    }
}
