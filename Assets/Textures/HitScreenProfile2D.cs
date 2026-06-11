using UnityEngine;
public class HitScreenProfile2D : ScriptableObject
{
    [Header("Overlay Assets")]
    public Sprite vignetteSprite;
    public Sprite bloodSprite;

    [Header("Vignette")]
    public Color vignetteColor = new Color(0.18f, 0.0f, 0.0f, 1f);
    [Range(0f, 1f)] public float lowHealthVignetteMaxAlpha = 0.35f;
    [Range(0f, 1f)] public float damageVignettePerHit = 0.18f;
    [Min(0f)] public float vignetteFadeOutSpeed = 1.75f;
    [Min(0f)] public float vignetteAttackSpeed = 18f;

    [Header("Blood Overlay")]
    public Color bloodColor = new Color(1f, 1f, 1f, 1f);
    [Range(0f, 1f)] public float lowHealthBloodMaxAlpha = 0.28f;
    [Range(0f, 1f)] public float damageBloodPerHit = 0.35f;
    [Min(0f)] public float bloodFadeOutSpeed = 1.2f;
    [Min(0f)] public float bloodAttackSpeed = 22f;

    [Header("Pulse")]
    public bool enableLowHealthPulse = true;
    [Range(0f, 1f)] public float pulseStartsBelowHealth01 = 0.45f;
    [Range(0f, 1f)] public float pulseMaxExtraAlpha = 0.12f;
    [Min(0.01f)] public float pulseFrequency = 2.6f;

    [Header("Hit Flash")]
    public bool enableFlash = true;
    public Color flashColor = new Color(0.85f, 0.0f, 0.0f, 1f);
    [Range(0f, 1f)] public float flashPerHit = 0.22f;
    [Min(0f)] public float flashFadeOutSpeed = 3.8f;

    [Header("Damage Scaling")]
    public bool scaleWithDamageAmount = true;
    [Min(0.01f)] public float referenceDamage = 20f;
    [Min(0f)] public float minDamageScale = 0.35f;
    [Min(0f)] public float maxDamageScale = 2f;

    [Header("Smoothing")]
    public bool useUnscaledTime = false;

    public float EvaluateDamageScale(float damage)
    {
        if (!scaleWithDamageAmount)
            return 1f;

        float scale = damage / Mathf.Max(0.01f, referenceDamage);
        return Mathf.Clamp(scale, minDamageScale, maxDamageScale);
    }
}
