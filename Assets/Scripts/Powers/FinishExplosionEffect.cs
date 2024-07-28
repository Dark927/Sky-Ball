using System.Collections.Generic;
using UnityEngine;

public class FinishExplosionEffect : MonoBehaviour
{
    List<ParticleSystem> _particleSystems;

    private void Awake()
    {
        _particleSystems = new(GetComponentsInChildren<ParticleSystem>());

    }

    private void Update()
    {
        foreach (ParticleSystem particles in _particleSystems)
        {
            if (particles.IsAlive())
            {
                return;
            }
        }

        Explosion sourceExplosion = GetComponentInParent<Explosion>();

        if (sourceExplosion != null)
        {
            sourceExplosion.gameObject.SetActive(false);
        }
    }
}
