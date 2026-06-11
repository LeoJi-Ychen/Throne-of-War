using UnityEngine;
using UnityEngine.UI;
public class HitScreen2D : MonoBehaviour
{
    GameObject player;
    [Header("Optional Profile")]
    [SerializeField] private HitScreenProfile2D profile;
    [SerializeField] private Image vignetteImage;
    [SerializeField] private Image bloodImage;
    [SerializeField] private Image flashImage;

    [Header("Runtime (Read Only)")]
    [SerializeField][Range(0f, 1f)] private float targetVignetteAlpha;
    [SerializeField][Range(0f, 1f)] private float currentVignetteAlpha;
    [SerializeField][Range(0f, 1f)] private float targetBloodAlpha;
    [SerializeField][Range(0f, 1f)] private float currentBloodAlpha;
    [SerializeField][Range(0f, 1f)] private float currentFlashAlpha;
    private float hitVignetteAdd;
    private float hitBloodAdd;
    private float pulseTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        ApplyImmediateVisual();
    }
    private void OnEnable()
    {
        Character.getDamage += ApplyHit;
    }
    private void OnDisable()
    {
        Character.getDamage -= ApplyHit;
    }
    // Update is called once per frame
    void Update()
    {
        if (profile == null)
            return;

        float dt = profile.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        float health01 = (float)player.GetComponent<Character>().blood/ player.GetComponent<Character>().maxblood;
        float lowHealthFactor = 1 - health01;

        hitVignetteAdd = Mathf.MoveTowards(hitVignetteAdd, 0f, profile.vignetteFadeOutSpeed * dt);
        hitBloodAdd = Mathf.MoveTowards(hitBloodAdd, 0f, profile.bloodFadeOutSpeed * dt);
        currentFlashAlpha = Mathf.MoveTowards(currentFlashAlpha, 0f, profile.flashFadeOutSpeed * dt);

        float pulseAlpha = 0f;
        if (profile.enableLowHealthPulse && health01 <= profile.pulseStartsBelowHealth01)
        {
            pulseTime += dt * profile.pulseFrequency;
            float normalized = 1f - Mathf.Clamp01(health01 / Mathf.Max(0.0001f, profile.pulseStartsBelowHealth01));
            float wave = 0.5f + 0.5f * Mathf.Sin(pulseTime * Mathf.PI * 2f);
            pulseAlpha = wave * profile.pulseMaxExtraAlpha * normalized;
        }
        else
        {
            pulseTime = 0f;
        }

        float baseVignette = lowHealthFactor * profile.lowHealthVignetteMaxAlpha;
        float baseBlood = lowHealthFactor * profile.lowHealthBloodMaxAlpha;

        targetVignetteAlpha = Mathf.Clamp01(baseVignette + hitVignetteAdd + pulseAlpha * 0.7f);
        targetBloodAlpha = Mathf.Clamp01(baseBlood + hitBloodAdd + pulseAlpha);

        currentVignetteAlpha = Mathf.MoveTowards(currentVignetteAlpha, targetVignetteAlpha, profile.vignetteAttackSpeed * dt);
        currentBloodAlpha = Mathf.MoveTowards(currentBloodAlpha, targetBloodAlpha, profile.bloodAttackSpeed * dt);
        ApplyImmediateVisual();
    }
    private void ApplyImmediateVisual()
    {
        if (profile == null)
            return;

        if (vignetteImage != null)
        {
            Color c = profile.vignetteColor;
            c.a *= currentVignetteAlpha;
            vignetteImage.color = c;
            vignetteImage.enabled = c.a > 0.001f || vignetteImage.sprite != null;
        }

        if (bloodImage != null)
        {
            Color c = profile.bloodColor;
            c.a *= currentBloodAlpha;
            bloodImage.color = c;
            bloodImage.enabled = c.a > 0.001f || bloodImage.sprite != null;
        }

        if (flashImage != null)
        {
            Color c = profile.flashColor;
            c.a *= currentFlashAlpha;
            flashImage.color = c;
            flashImage.enabled = profile.enableFlash && c.a > 0.001f;
        }
    }
    public void ApplyHit(float damageAmount = 10f)
    {
        if (!SwitchPerspective.isFP)
        {
            return;
        }
        bool addBlood = true;
        bool addVignette = true;
        bool addFlash = true;
        if (profile == null)
            return;

        float scale = profile.EvaluateDamageScale(Mathf.Max(0.01f, damageAmount));

        if (addVignette)
            hitVignetteAdd = Mathf.Clamp01(hitVignetteAdd + profile.damageVignettePerHit * scale);

        if (addBlood)
            hitBloodAdd = Mathf.Clamp01(hitBloodAdd + profile.damageBloodPerHit * scale);

        if (addFlash && profile.enableFlash)
            currentFlashAlpha = Mathf.Clamp01(currentFlashAlpha + profile.flashPerHit * scale);

        ApplyImmediateVisual();
    }
}
