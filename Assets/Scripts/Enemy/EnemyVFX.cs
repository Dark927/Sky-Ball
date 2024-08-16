using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _deathExplosionVFX;

    public void Explode(Vector3 position)
    {
        _deathExplosionVFX.gameObject.SetActive(true);
        transform.position = position;
        _deathExplosionVFX.Play();
    }
}
