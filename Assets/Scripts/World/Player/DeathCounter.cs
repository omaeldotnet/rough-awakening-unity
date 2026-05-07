using TMPro;
using UnityEngine;

public class DeathCounter : MonoBehaviour
{
    public static Score instance;

    public TextMeshProUGUI deathsText;
    public float deathCount;


    private void Start()
    {
        deathCount = PlayerPrefs.GetFloat("Death", 0); // Load saved death count, if no, 0 deaths
        Debug.LogWarning("death count is " + deathCount);
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Y)) // Press 'Y' to test
        {
            ResetDeath();
        }

        if (deathsText != null)
        {
            deathsText.text = "DEATHS: " + Mathf.RoundToInt(deathCount).ToString();
        }
    }

    public void AddDeath()
    {
        deathCount += 1;
        PlayerPrefs.SetFloat("Death", deathCount);
        Debug.LogWarning("death count is " + deathCount);
    }

    public void ResetDeath()
    {
        deathCount = 0;
        PlayerPrefs.SetFloat("Death", deathCount);
        Debug.LogWarning("death count is " + deathCount);
    }

}
