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
    private Rigidbody rb;
    private Animator anim;

    // Damage colors
    [SerializeField] private Color normalDamageColor;
    [SerializeField] private Color criticalDamageColor;
    [Range(0, 2)][SerializeField] private float offset;
    [Range(50, 500)][SerializeField] private float speed;


    private void Awake()
    {
        damageText = GetComponent<TextMesh>();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void OnDisable()
    {
        // Sets kinematic to disable every force
        rb.isKinematic = true;
        anim.ResetTrigger("NormalHit");
        anim.ResetTrigger("CriticalHit");
    }

    /// <summary>
    /// Updates text.
    /// </summary>
    /// <param name="damage">Damage to print.</param>
    /// <param name="criticalHit">Critical hit.</param>
    public void UpdateShownDamage(float damage, bool criticalHit)
    {
        rb.isKinematic = false;

        // Spawn random position
        float additionalOffset = Random.Range(-offset, offset);
        transform.position += new Vector3(additionalOffset, offset, additionalOffset);

        float movementSpeed = Random.Range(-speed, speed);
        rb.AddForce(new Vector3(movementSpeed * 0.25f, Mathf.Abs(speed), movementSpeed * 0.25f));

        // Update text and color
        damageText.text = damage.ToString();

        // Updates color and animation depending on critical hit
        if (criticalHit == false)
        {
            anim.SetTrigger("NormalHit");
            damageText.color = normalDamageColor; 
        }
        else
        {
            anim.SetTrigger("CriticalHit");
            damageText.color = criticalDamageColor;
        } 
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
