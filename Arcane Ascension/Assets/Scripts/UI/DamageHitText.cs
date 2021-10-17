using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for updating spawn damage hit numbers and colors.
/// </summary>
sealed public class DamageHitText : MonoBehaviour
{
    // Components
    private TextMesh damageText;
    private Camera mainCamera;

    // Damage colors
    [SerializeField] private Color normalDamageColor;
    [SerializeField] private Color criticalDamageColor;
    [Range(0, 2)][SerializeField] private float offset;
    [Range(50, 500)][SerializeField] private float speed;

    private Rigidbody rb;

    private void Awake()
    {
        damageText = GetComponent<TextMesh>();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    /// <summary>
    /// Updates text.
    /// </summary>
    /// <param name="damage">Damage to print.</param>
    /// <param name="criticalHit">Critical hit.</param>
    public void UpdateShownDamage(float damage, bool criticalHit)
    {
        // Spawn random position
        float additionalOffset = Random.Range(-offset, offset);
        transform.position += new Vector3(additionalOffset, offset, additionalOffset);

        float movementSpeed = Random.Range(-speed, speed);
        rb.AddForce(new Vector3(movementSpeed * 0.25f, Mathf.Abs(speed), movementSpeed * 0.25f));

        // Update text and color
        damageText.text = damage.ToString();
        damageText.color = 
            criticalHit ? 
            damageText.color = criticalDamageColor : 
            damageText.color = normalDamageColor;
    }


    /// <summary>
    /// Rotates text to camera.
    /// </summary>
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.Direction(transform.position), Vector3.up);
    }

    public void DisableAnimationEvent() =>
        gameObject.SetActive(false);
}
