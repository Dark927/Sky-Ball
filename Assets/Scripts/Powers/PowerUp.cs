using UnityEngine;



public class PowerUp : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Enums
    // -----------------------------------------------------------------------

    #region Enums

    public enum TYPE
    {
        Strength = 0,
        Rockets = 1,
        Explosion,
        MassUP,

        FirstIndex = 0,
        LastIndex = MassUP,
        NumberOfPowers = LastIndex + 1,
    }

    #endregion


    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [SerializeField] private TYPE _powerType = TYPE.FirstIndex;

    [SerializeField] private float _powerUpActiveTime = 5f;
    [SerializeField] private float _powerUpStrength = 5f;
    [SerializeField] private float _powerUpReloadTime = 1f;

    [SerializeField] private Color _indicatorColor;


    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public TYPE GetPowerType()
    {
        if (_powerType != TYPE.NumberOfPowers)
        {
            return _powerType;
        }
        else
        {
            Debug.Log($"# Warning -> {gameObject.name} - PowerUpType == Power_numberOfPowers. Return LAST power up.");
            return TYPE.LastIndex;
        }
    }

    public float GetPowerActiveTime()
    {
        return _powerUpActiveTime;
    }

    public float GetPowerReloadTime()
    {
        return _powerUpReloadTime;
    }

    public float GetPowerUpStrength()
    {
        return _powerUpStrength;
    }

    public Color GetIndicatorColor()
    {
        return _indicatorColor;
    }    

    #endregion
}
