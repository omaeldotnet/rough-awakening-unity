using UnityEngine;

public class Respawn : MonoBehaviour
{
    private void Start() //just pour set le premier checkpoint dans la maison
    {
        float startX = gameObject.transform.position.x;
        float startY = gameObject.transform.position.y;
        float startZ = gameObject.transform.position.z;

        PlayerPrefs.SetFloat("PlayerX", startX); //pas besoin get set, set va create le player pref
        PlayerPrefs.SetFloat("PlayerY", startY);
        PlayerPrefs.SetFloat("PlayerZ", startZ);
    }
    public void LastCheckPoint()
    {
        float lastX = PlayerPrefs.GetFloat("PlayerX"); //get derniere position, si pas checkpoint, starter house
        float lastY = PlayerPrefs.GetFloat("PlayerY");
        float lastZ = PlayerPrefs.GetFloat("PlayerZ");
        transform.position = new Vector3(lastX, lastY, lastZ);
    }

    
}
