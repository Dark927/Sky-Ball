using System.Collections.Generic;
using UnityEngine;

public class FinishDefaultEffect : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    private List<ParticleSystem> _particleSystems;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _particleSystems = new(GetComponentsInChildren<ParticleSystem>());
    }

    private void Update()
    {
        if (IsParticlesAlive()) return;

        SetFinishOptions();
    }

    #endregion


    // -----------------------------------------------------------------------
    // Protected Methods
    // -----------------------------------------------------------------------

    #region Protected Methods

    protected virtual void SetFinishOptions()
    {
        gameObject.SetActive(false);
    }

    protected bool IsParticlesAlive()
    {
        foreach (ParticleSystem particles in _particleSystems)
        {
            if (particles.IsAlive())
            {
                return true;
            }
        }
        return false;
    }

    #endregion
}
