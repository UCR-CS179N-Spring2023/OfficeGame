using System.Collections;
using System.Collections.Generic;
using DamageMechanics;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Transform enemyTransform;
    public Image fillImage;
    public Image damageIndicatorImage; 
    public Image backgroundImage;
    public GameObject tickMarkPrefab;

    public float animationTime = 0.75f;

    private DamageMechanics.Damageable damageable; 
    private RectTransform HealthBar;
    private RectTransform DamageIndicator;
    private float initialWidth;
    private float initialOffset; // offset needed to align the health bar with the center of the enemy's head.

    private float previousHealth;

    private void Awake()
    {
        damageable = GetComponentInParent<DamageMechanics.Damageable>();
        HealthBar = fillImage.GetComponent<RectTransform>();
        DamageIndicator = damageIndicatorImage.GetComponent<RectTransform>();

        if (damageable == null) Debug.LogError("EnemyHealthBar.cs: No Damageable component found in parent object.");
        if (HealthBar == null) Debug.LogError("EnemyHealthBar.cs: No RectTransform component found in fillImage object.");
        if (DamageIndicator == null) Debug.LogError("EnemyHealthBar.cs: No RectTransform component found in damageIndicatorImage object.");

        initialWidth = HealthBar.rect.width;
        initialOffset = initialWidth / 2f;

        previousHealth = damageable.Health;
        SetHealth(damageable.Health);
        CreateTickMarks(new float[] {0.125f, 0.25f, 0.375f, 0.5f, 0.625f, 0.75f, 0.875f});

        damageable.OnHit.AddListener(OnEnemyHit);
    }
    
    private void Update()
    {
        if (enemyTransform == null || fillImage == null) return;

        Vector3 enemyHeadPosition = enemyTransform.position + Vector3.up * 2f;
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(enemyHeadPosition);

        screenPosition.x -= initialOffset;

        fillImage.transform.position = screenPosition + new Vector2(4f, 0f); // 4f is the offset from the left side of the screen (shifting right). this is needed to align the health bar with the background image.
        damageIndicatorImage.transform.position = screenPosition + new Vector2(4f, 0f);
        backgroundImage.transform.position = screenPosition;
    }

    private Coroutine currentAnimationCoroutine = null;

    public void SetHealth(float health)
    {
        float fillAmount = damageable.Health / damageable.InitHealth;
        float newWidth = initialWidth * fillAmount;

        float damageTaken = (previousHealth - damageable.Health) / damageable.InitHealth;
        float damageTakenWidth = initialWidth * damageTaken;

        HealthBar.sizeDelta = new Vector2(newWidth, HealthBar.rect.height);

        previousHealth = damageable.Health;

        if (currentAnimationCoroutine != null) 
        {
            StopCoroutine(currentAnimationCoroutine);
            currentAnimationCoroutine = null;
        }
        currentAnimationCoroutine = StartCoroutine(AnimateDamageIndicator(newWidth, damageTakenWidth, animationTime));

        Vector3 enemyHeadPosition = enemyTransform.position + Vector3.up * 2f;
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(enemyHeadPosition);

        screenPosition.x -= initialOffset;

        fillImage.transform.position = screenPosition + new Vector2(4f, 0f);
        damageIndicatorImage.transform.position = screenPosition + new Vector2(4f, 0f);
        backgroundImage.transform.position = screenPosition;
    }

    private IEnumerator AnimateDamageIndicator(float targetWidth, float damageTakenWidth, float animationTime)
    {   
        float startWidth = targetWidth + damageTakenWidth;
       
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

    public void CreateTickMarks(float[] percentages)
    {
        foreach (float percentage in percentages)
        {
            float tickmarkPosX = initialWidth * percentage;
            GameObject tickMark = Instantiate(tickMarkPrefab, HealthBar.transform);
            RectTransform tickMarkTransform = tickMark.GetComponent<RectTransform>();
            tickMarkTransform.localPosition = new Vector3(tickmarkPosX, 0f, 0f);
            tickMarkTransform.localScale = Vector3.one;

            float tickmarkSize = HealthBar.rect.height;
            tickMarkTransform.sizeDelta = new Vector2(2f, tickmarkSize);
            tickMarkTransform.pivot = new Vector2(0.5f, -0.5f); // position the tick marks at the top of the health bar.
        }
    }

    public void OnEnemyHit(Damage damage)
    {
        SetHealth(damageable.Health);
        // Debug.Log("Enemy health changed. Current Health: " + damageable.Health);
    }
}