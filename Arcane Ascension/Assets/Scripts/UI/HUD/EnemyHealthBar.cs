using UnityEngine;

/// <summary>
/// Class responsible for controling enemy health bar information.
/// </summary>
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject healthBarGameObject;
    [SerializeField] private RectTransform healthBarImage;

    // Components
    private Camera cam;
    private EnemyStats enemyStats;

    private void Awake()
    {
        cam = Camera.main;
        enemyStats = GetComponentInParent<EnemyStats>();
    }

    private void Start()
    {
        healthBarGameObject.SetActive(false);
    }

    private void OnEnable()
    {
        enemyStats.EventTakeDamage += UpdateInformation;
    }

    private void OnDisable()
    {
        enemyStats.EventTakeDamage -= UpdateInformation;
    }

    private void UpdateInformation(float damageToTake)
    {
        healthBarImage.sizeDelta = 
            new Vector2((enemyStats.Health-damageToTake) / enemyStats.MaxHealth, healthBarImage.sizeDelta.y);
    }

    public void EnableEnemyHealthBar(bool condition)
    {
        if (condition)
        {
            healthBarGameObject.SetActive(true);
            healthBarImage.sizeDelta = new Vector2(enemyStats.Health / enemyStats.MaxHealth, healthBarImage.sizeDelta.y);
        }
        else
        {
            healthBarGameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        if (healthBarGameObject)
            healthBarGameObject.transform.LookAt(cam.transform);
    }
}
