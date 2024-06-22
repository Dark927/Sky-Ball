using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters 

    [SerializeField] float radius = 14f;
    [SerializeField] float upwardsModifier = 1f;

    FadeOutColor fadeOut;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods 

    private void Awake()
    {
        fadeOut = GetComponent<FadeOutColor>();
    }


    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void Explode(float force)
    {
        fadeOut.StartFadeOut();

        Collider[] collidersList = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in collidersList)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();

                float massMultiplier = (enemyRb.mass < 1f) ? 1f : enemyRb.mass;
                float actualForce = force * massMultiplier;
                float actualUpwardsModifier = upwardsModifier * (massMultiplier / 2f);

                enemyRb.AddExplosionForce(actualForce, transform.position, radius, actualUpwardsModifier, ForceMode.Impulse);
            }
        }

        //Destroy(gameObject);
    }

    #endregion
}
