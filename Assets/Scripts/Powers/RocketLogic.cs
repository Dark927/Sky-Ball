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

    private Transform _target;
    private Vector3 _rocketDirection;
    private Vector3 _explodeBound = new(0, -0.7f, 0);

    [SerializeField] private OWNER _rocketOwner = OWNER.FirstIndex;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _rocketForce = 2f;
    [SerializeField] private bool _autoAiming = false;


    #endregion

    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

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
        if ((_target == null) || (transform.position.y < _explodeBound.y))
        {
            Explode();
            return;
        }

        RocketFly(_target);
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

    private void OnTriggerEnter(Collider target—ollider)
    {
        switch (_rocketOwner)
        {
            case OWNER.Player:
                {
                    PushTarget<Enemy>(target—ollider);
                }
                break;


            case OWNER.Enemy:
                {
                    PushTarget<PlayerController>(target—ollider);
                }
                break;
        }
    }

    private void PushTarget<Target>(Collider targetCollider) where Target : MonoBehaviour
    {
        Target target = targetCollider.GetComponent<Target>();

        if (target != null)
        {
            Rigidbody targetRb = target.GetComponent<Rigidbody>();
            Vector3 pushDirection = (target.transform.position - transform.position).normalized;

            targetRb.AddForce(pushDirection * _rocketForce, ForceMode.Impulse);
            Explode();
        }
    }

    private void Explode()
    {
        Destroy(gameObject);
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
