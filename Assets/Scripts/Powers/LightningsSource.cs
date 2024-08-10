using UnityEngine;
using System;

public class LightningsSource : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [Header("Lightnings Settings")]
    [Space]

    [SerializeField] private float _explosionDelay = 1.4f;

    private ObjectsPool _lightningsPool;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _lightningsPool = GetComponent<ObjectsPool>();
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void ActivateLightnings<Target>(float force, int minStrikesCount, int maxStrikesCount) where Target : MonoBehaviour
    {
        if (_lightningsPool == null)
        {
            throw new NullReferenceException($"{gameObject.name} obj. : {nameof(_lightningsPool)} - null, can't create lightnings.");
        }

        int lightningsCount = UnityEngine.Random.Range(minStrikesCount, maxStrikesCount);
        Target[] allTargets = FindObjectsOfType<Target>();

        for (int currentIndex = 0; (currentIndex < lightningsCount) && (currentIndex < allTargets.Length); ++currentIndex)
        {
            GameObject lightningStrike = _lightningsPool.RequestInactiveObject(true);

            lightningStrike.transform.position = allTargets[currentIndex].transform.position;
            lightningStrike.SetActive(true);

            Explosion strikeExplosion = lightningStrike.GetComponent<Explosion>();
            strikeExplosion.Explode(force, _explosionDelay);
        }
    }

    #endregion
}
