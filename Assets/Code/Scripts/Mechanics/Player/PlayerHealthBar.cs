using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{  
    // Current implementation of PlayerHealthBar is a green horizontal bar 'image' that scales in width.
    // Underneath is a red horizontal bar 'image' that scales in width to indicate damage taken.
    public Image fillImage;
    public Image damageIndicatorImage; 
    public GameObject tickMarkPrefab;

    // Animation time is the time it takes for the damage indicator to fade out.
    public float animationTime = 0.75f; 
    // private float animationCooldown = 0.2f;
    // private bool isAnimating = false;

    // Damageable is only used to get current health of the player.
    private DamageMechanics.Damageable damageable; 
    private RectTransform HealthBar;
    private RectTransform DamageIndicator;
    private float initialWidth;

    // Stores the current health of the player.
    private float _health;

    private void Start()
    {
        // Get components.
        damageable = GetComponentInParent<DamageMechanics.Damageable>();
        HealthBar = fillImage.GetComponent<RectTransform>();
        DamageIndicator = damageIndicatorImage.GetComponent<RectTransform>();

        if (damageable == null) Debug.LogError("PlayerHealthBar.cs: No Damageable component found in parent object.");
        if (HealthBar == null) Debug.LogError("PlayerHealthBar.cs: No RectTransform component found in fillImage object.");
        if (DamageIndicator == null) Debug.LogError("PlayerHealthBar.cs: No RectTransform component found in damageIndicatorImage object.");

        initialWidth = HealthBar.rect.width;
        SetMaxHealth();
        CreateTickMarks(new float[] {0.125f, 0.25f, 0.375f, 0.5f, 0.625f, 0.75f, 0.875f});

        _health = damageable.Health;
    }

    private void Update()
    {
        // If the player's health has changed, update the health bar.
        if (damageable.Health != _health)
        {
            SetHealth();
            _health = damageable.Health;
        }
        
    }

    // HealthBar functions.
    public void SetMaxHealth()
    {
        HealthBar.sizeDelta = new Vector2(initialWidth, HealthBar.rect.height);
    }

    private Coroutine currentAnimationCoroutine = null;

    public void SetHealth()
    {
        // if (isAnimating) return;

        float fillAmount = damageable.Health / damageable.InitHealth;
        float newWidth = initialWidth * fillAmount;

        float damageTaken = (_health - damageable.Health) / damageable.InitHealth;
        float damageTakenWidth = initialWidth * damageTaken;

        // Debug.Log("SetHealth() called. Health: " + damageable.Health + ", Fill Amount: " + fillAmount);
        // Debug.Log("Damage Taken: " + damageTaken + ", Damage Taken Width: " + damageTakenWidth);

        HealthBar.sizeDelta = new Vector2(newWidth, HealthBar.rect.height);
        
        // Only animates the last damage taken.
        if (currentAnimationCoroutine != null) 
        {
            StopCoroutine(currentAnimationCoroutine);
            currentAnimationCoroutine = null;
        }

        currentAnimationCoroutine = StartCoroutine(AnimateDamageIndicator(newWidth, damageTakenWidth, animationTime));
        // StartCoroutine(AnimationCooldown());
    }

    public void CreateTickMarks(float[] percentages)
    {
        foreach (float percentage in percentages)
        {
            float tickmarkPosX = initialWidth * percentage;
            GameObject tickMark = Instantiate(tickMarkPrefab, HealthBar.transform);
            RectTransform tickMarkTransform = tickMark.GetComponent<RectTransform>();
            tickMarkTransform.localPosition = new Vector3(tickmarkPosX, 0f, 0f);
        }
    }

    private IEnumerator AnimateDamageIndicator(float targetWidth, float damageTakenWidth, float animationTime)
    {   
        float startWidth = targetWidth + damageTakenWidth;
        // Debug.Log("AnimateDamageIndicator() called. Start Width: " + startWidth + ", Target Width: " + targetWidth + ", Damage Taken Width: " + damageTakenWidth + ", Animation Time: " + animationTime);

        float time = 0f;
        while (time < animationTime)
        {
            float width = Mathf.Lerp(startWidth, targetWidth, time / animationTime);
            DamageIndicator.sizeDelta = new Vector2(width, DamageIndicator.rect.height);
            time += Time.deltaTime;
            yield return null;
        }

        DamageIndicator.sizeDelta = new Vector2(targetWidth, DamageIndicator.rect.height);
    }

    /*
    private IEnumerator AnimationCooldown()
    {
        isAnimating = true;
        yield return new WaitForSeconds(animationCooldown);
        isAnimating = false;
    }
    */
}
