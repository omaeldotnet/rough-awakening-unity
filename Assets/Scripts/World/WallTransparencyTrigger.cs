using UnityEngine;
using System.Collections;

public class WallTransparencyTrigger : MonoBehaviour
{
    private void Start()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("InvisibleWall");
        foreach (GameObject wall in walls)
        {
            FadeWall(wall, 0f, 0f); // fade out quickly on load
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InvisibleWall"))
        {
            FadeWall(other.gameObject, 0.1f, 0.3f); // Fade to 10%
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InvisibleWall"))
        {
            FadeWall(other.gameObject, 0f, 0.3f); // Fade back to 0%
        }
    }

    private void FadeWall(GameObject wall, float targetAlpha, float duration)
    {
        StartCoroutine(FadeWallCoroutine(wall, targetAlpha, duration));
    }

    private IEnumerator FadeWallCoroutine(GameObject wall, float targetAlpha, float duration)
    {
        Renderer renderer = wall.GetComponent<Renderer>();
        if (renderer == null) yield break;

        Material mat = renderer.material;

        SetMaterialTransparent(mat); // 🟢 Apply transparency settings before fading

        Color color = mat.color;
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            color.a = newAlpha;
            mat.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        mat.color = color;
    }

    // ✅ Correct transparency setup (no recursion!)
    void SetMaterialTransparent(Material mat)
    {
        mat.SetOverrideTag("RenderType", "Transparent");
        mat.SetFloat("_Mode", 3); // Transparent mode
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }
}
