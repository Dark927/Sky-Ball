using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields 

    [SerializeField] private float _radius = 14f;
    [SerializeField] private float _upwardsModifier = 0.75f;

    private FadeOutColor _fadeOut;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void Explode(float force, float timeDelay = 0f)
    {
        StartCoroutine(ExplosionRoutine(force, timeDelay));
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods 

    private void Awake()
    {
        _fadeOut = GetComponent<FadeOutColor>();
    }
    
    private IEnumerator ExplosionRoutine(float force, float timeDelay = 0f)
    {
        yield return new WaitForSeconds(timeDelay);

        TryFadeOut();

        Collider[] collidersList = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider collider in collidersList)
        {
            EnemyBody enemy = collider.GetComponent<EnemyBody>();

            if (enemy != null)
            {
                PushEnemy(enemy, force);
            }
        }
    }

    private void TryFadeOut()
    {
        if (_fadeOut != null)
        {
            _fadeOut.StartFadeOut();
        }
    }

    private void PushEnemy(EnemyBody enemy, float force)
    {
        Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();

        float massMultiplier = (enemyRb.mass < 1f) ? 1f : enemyRb.mass;
        float actualForce = force * massMultiplier;
        float actualUpwardsModifier = _upwardsModifier * (massMultiplier / 2f);

        enemyRb.AddExplosionForce(actualForce, transform.position, _radius, actualUpwardsModifier, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    #endregion
}
