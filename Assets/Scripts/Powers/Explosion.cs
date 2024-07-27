using UnityEngine;

public class Explosion : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields 

    [SerializeField] private float _radius = 14f;
    [SerializeField] private float _upwardsModifier = 1f;

    private FadeOutColor _fadeOut;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods 

    private void Awake()
    {
        _fadeOut = GetComponent<FadeOutColor>();
    }


    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void Explode(float force)
    {
        _fadeOut.StartFadeOut();

        Collider[] collidersList = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider collider in collidersList)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                PushEnemy(enemy, force);
            }
        }

        //Destroy(gameObject);
    }

    private void PushEnemy(Enemy enemy, float force)
    {
        Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();

        float massMultiplier = (enemyRb.mass < 1f) ? 1f : enemyRb.mass;
        float actualForce = force * massMultiplier;
        float actualUpwardsModifier = _upwardsModifier * (massMultiplier / 2f);

        enemyRb.AddExplosionForce(actualForce, transform.position, _radius, actualUpwardsModifier, ForceMode.Impulse);
    }

    #endregion
}
