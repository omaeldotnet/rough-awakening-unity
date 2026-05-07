using System.Collections;
using UnityEngine;

public class ZoneTriggers : MonoBehaviour
{
    string zoneName;
    string zombieTag;
    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Player"))
        {
            Debug.LogWarning("Player entered zibe trigger");
            zoneName = gameObject.name; //name of trigger, for zombies in zone Zone2Trigger
            
            switch (zoneName)
            {
                case "InitialZoneTrigger":
                    zombieTag = "InitialZone";
                    break;
                case "Zone1Trigger":
                    zombieTag = "Zone1";
                    break;
                case "Zone2Trigger":
                    zombieTag = "Zone2";
                    break;
                case "Zone3Trigger":
                    zombieTag = "Zone3";
                    break;
            }
            

            // Activate all zombies with matching zone tag
            GameObject[] zoneZombies = GameObject.FindGameObjectsWithTag(zombieTag); //list de zombie gameobjects
            foreach (GameObject zombie in zoneZombies)
            {
                zombie.GetComponent<ZombieFollow>().FollowPlayer();
            }
        }
    }
}