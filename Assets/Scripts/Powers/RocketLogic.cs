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
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        _projectileVFX.SetActive(true);
    }

    private void Start()
    {
        if (_target != null)
        {
            CalculateAutoAim(_target);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (_exploded)
        {
            ConfigureActiveStates();
            return;
        }


        bool readyToExplode = ((_target == null) || (transform.position.y < _explodeBound.y));

        if (readyToExplode) Explode();
        else RocketFly(_target);
    }

    private void ConfigureActiveStates()
    {
        if (!IsRocketEffectsActive())
        {
            _hitVFX.SetActive(false);
            gameObject.SetActive(false);
            _exploded = false;
            _collider.enabled = true;
        }
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

        transform.position += (_rocketDirection * _speed * Time.deltaTime);
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
                    PushTarget<Enemy>(targetObject);
                }
                break;


            case OWNER.Enemy:
                {
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
    }

    private void Explode()
    {
        SetExplosionSettings();
        PlayHitVFX(transform.position, transform.rotation);
    }

    private void SetExplosionSettings()
    {
        _exploded = true;
        _collider.enabled = false;
        _projectileVFX.SetActive(false);
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

    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    #endregion
}
