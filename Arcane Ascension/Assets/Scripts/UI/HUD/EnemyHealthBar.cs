using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

/// <summary>
/// Class responsible for controling enemy health bar information.
/// </summary>
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject healthBarGameObject;
    [SerializeField] private Image health;

    [Header("Health bar")]
    [SerializeField] private Color lowHealth;
    [SerializeField] private Color highHealth;

    // Components
    private Camera cam;
    private EnemyStats enemyStats;

    // Coroutines
    private IEnumerator updateHealthCoroutine;
    private YieldInstruction wffu;

    private void Awake()
    {
        cam = Camera.main;
        enemyStats = GetComponentInParent<EnemyStats>();
        wffu = new WaitForFixedUpdate();
    }

    private void Start()
    {
        healthBarGameObject.SetActive(false);
    }

    private void OnEnable()
    {
        enemyStats.EventTakeDamage += OnTakeDamage;
    }

    private void OnDisable()
    {
        enemyStats.EventTakeDamage -= OnTakeDamage;
    }

    /// <summary>
    /// Starts a coroutine to update health.
    /// </summary>
    private void OnTakeDamage() =>
         this.StartCoroutineWithReset(ref updateHealthCoroutine, UpdateHealthCoroutine());

    /// <summary>
    /// Coroutine that updates health bar UI fill amount and color smoothly.
    /// </summary>
    /// <returns>WFFU.</returns>
    private IEnumerator UpdateHealthCoroutine()
    {
        do
        {
            if (enemyStats == null)
                break;

            health.fillAmount =
                Mathf.Lerp(
                    health.fillAmount,
                    enemyStats.Health / enemyStats.CommonAttributes.MaxHealth,
                    Time.fixedDeltaTime * 5f);

            health.color =
                health.color.Remap(
                    0.3f, 0.5f, lowHealth,
                    highHealth, health.fillAmount);

            yield return wffu;
        }
        while (enemyStats.Health.Similiar(health.fillAmount) == false);
    }

    /// <summary>
    /// Enables or disables health bar.
    /// </summary>
    /// <param name="condition">True to enable, false to disable.</param>
    public void EnableEnemyHealthBar(bool condition)
    {
        if (condition)
        {
            healthBarGameObject.SetActive(true);
            OnTakeDamage();
        }
        else
        {
            healthBarGameObject.SetActive(false);
        }
    }

    private void FixedUpdate() =>
        UpdateRotation();

    /// <summary>
    /// Updates rotation of the bar.
    /// </summary>
    private void UpdateRotation()
    {
        if (healthBarGameObject.activeSelf)
            healthBarGameObject.transform.LookAt(cam.transform);
    }
}
