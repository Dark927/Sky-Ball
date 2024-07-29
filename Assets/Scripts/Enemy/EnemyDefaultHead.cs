using UnityEngine;

public class EnemyDefaultHead : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Enums
    // -----------------------------------------------------------------------

    #region Enums

    public enum TYPE
    {
        Default = 0,
        Heavy,
        Fast,
        Boss,
        Range,

        Group,

        FirstIndex = 0,
        LastIndex = Group,
        NumberOfEnemy = LastIndex + 1,
    }

    #endregion

    // -----------------------------------------------------------------------
    // Fields 
    // -----------------------------------------------------------------------

    #region Fields

    [Header("Main Settings")]

    [SerializeField] private TYPE _type;
    protected EnemyBody _body;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods



    public void Deactivate()
    {
        // TODO : Add score for player 

        gameObject.SetActive(false);
    }

    public TYPE Type
    {
        get
        {
            if (!IsCorrectType())
            {
                _type = TYPE.LastIndex;
                Debug.Log($"# Warning -> {gameObject.name} - Type == NumberOfEnemy. Return LAST enemy type.");
            }

            return _type;
        }
    }


    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _body = GetComponentInChildren<EnemyBody>();
    }
    private bool IsCorrectType() => (_type != TYPE.NumberOfEnemy);

    #endregion
}
