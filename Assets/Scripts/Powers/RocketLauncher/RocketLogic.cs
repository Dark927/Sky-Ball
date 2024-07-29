using System.Collections;
using UnityEngine;

public class RocketLogic : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Enums

    public enum OWNER
    {
        Player = 0,
        Enemy,

        FirstIndex = 0,
        LastIndex = Enemy,
        NumberOfRockets = LastIndex + 1,
    }

    #endregion


    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields 

    [Header("Main Settings")]
    [Space]

    [SerializeField] private OWNER _rocketOwner = OWNER.FirstIndex;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _rocketForce = 2f;
    [SerializeField] private bool _autoAiming = false;
    [SerializeField] private float _forwardOffsetMultiplier = 1.1f;

    [Header("VFX Settings")]
    [Space]

    [SerializeField] private GameObject _projectileVFX;
    [SerializeField] private GameObject _hitVFX;


    private Transform _target;
    private Collider _collider;
    private Vector3 _rocketDirection;
    private Vector3 _explodeBound = new(0, -0.7f, 0);

    private bool _exploded = false;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void OnEnable()
    {
        if (_target != null)
        {
            CalculateAutoAim(_target);
        }

        _projectileVFX.SetActive(true);
    }

    private void Start()
    {
        if (_target != null)
        {
            CalculateAutoAim(_target);
        }

        _projectileVFX.SetActive(true);
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (_exploded) return;

        bool readyToExplode = ((_target == null) || (transform.position.y < _explodeBound.y));

        if (readyToExplode) Explode();
        else RocketFly(_target);
    }

    private bool IsRocketEffectsActive()
    {
        if (_hitVFX.activeInHierarchy)
        {
            ParticleSystem[] allParticles = _hitVFX.GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem particles in allParticles)
            {
                if (particles.IsAlive())
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void RocketFly(Transform target)
    {
        if (_autoAiming)
        {
            CalculateAutoAim(target);
        }

        transform.Translate(_rocketDirection * _speed * Time.deltaTime, Space.World);
    }

    private void CalculateAutoAim(Transform target)
    {
        _rocketDirection = CalculateDirection(target);
        ConfigureRotation(target);
    }

    private Vector3 CalculateDirection(Transform target)
    {
        return (target.position - transform.position).normalized;
    }

    private void ConfigureRotation(Transform target)
    {
        transform.LookAt(target);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject targetObject = collision.gameObject;

        switch (_rocketOwner)
        {
            case OWNER.Player:
                {
                    if (HasComponentOfType<PlayerController>(targetObject)) return;

                    PushTarget<EnemyBody>(targetObject);
                }
                break;


            case OWNER.Enemy:
                {
                    if (HasComponentOfType<EnemyBody>(targetObject)) return;

                    PushTarget<PlayerController>(targetObject);
                }
                break;
        }

        // Explode rocket 

        ContactPoint contactPoint = collision.contacts[0];
        Vector3 position = contactPoint.point;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);

        Explode(position, rotation);
    }

    private bool HasComponentOfType<Type>(GameObject targetObject) where Type : MonoBehaviour
    {
        return (targetObject.GetComponent<Type>() != null);
    }

    private void PushTarget<Target>(GameObject targetObject) where Target : MonoBehaviour
    {
        Target target = targetObject.GetComponent<Target>();

        if (target != null)
        {
            Rigidbody targetRb = target.GetComponent<Rigidbody>();
            Vector3 pushDirection = (target.transform.position - transform.position).normalized;

            targetRb.AddForce(pushDirection * _rocketForce * targetRb.mass, ForceMode.Impulse);
        }
    }

    private void Explode(Vector3 position, Quaternion rotation)
    {
        SetExplosionSettings();
        PlayHitVFX(position, rotation);
        StartCoroutine(ConfigureActiveStates());
    }

    private void Explode()
    {
        SetExplosionSettings();
        PlayHitVFX(transform.position, transform.rotation);
        StartCoroutine(ConfigureActiveStates());
    }

    private void SetExplosionSettings()
    {
        _exploded = true;
        _collider.enabled = false;
        _projectileVFX.SetActive(false);
    }

    private IEnumerator ConfigureActiveStates()
    {
        while (IsRocketEffectsActive())
        {
            yield return null;
        }

        ResetSettings();
    }

    private void ResetSettings()
    {
        _hitVFX.SetActive(false);
        gameObject.SetActive(false);
        _exploded = false;

        _collider.enabled = true;
    }

    private void PlayHitVFX(Vector3 position, Quaternion rotation)
    {
        if (_hitVFX != null)
        {
            position += transform.forward * _forwardOffsetMultiplier;

            _hitVFX.transform.position = position;
            _hitVFX.transform.rotation = rotation;
            _hitVFX.SetActive(true);
        }
    }

    #endregion
}
