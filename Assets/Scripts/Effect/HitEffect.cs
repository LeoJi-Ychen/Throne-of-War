using UnityEngine;
using System.Collections;

public class HitEffect : MonoBehaviour
{
    private Renderer[] renderers;
    private Color[] originalColors;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

    public void TakeDamage()
    {
        StartCoroutine(HitFlash());
    }

    IEnumerator HitFlash()
    {
        float duration = 0.12f;
        float timer = 0f;

        foreach (Renderer r in renderers)
        {
            r.material.EnableKeyword("_EMISSION");
            r.material.SetColor("_EmissionColor", Color.red * 8f);
        }

        yield return new WaitForSeconds(0.04f);

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            float intensity = Mathf.Lerp(8f, 0f, t * t);

            foreach (Renderer r in renderers)
            {
                r.material.SetColor("_EmissionColor", Color.red * intensity);
            }

            yield return null;
        }

        foreach (Renderer r in renderers)
        {
            r.material.SetColor("_EmissionColor", Color.black);
        }
    }
}