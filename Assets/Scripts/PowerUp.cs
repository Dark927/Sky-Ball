using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    Power_strengthUP = 0,
    Power_rockets = 1,
    Power_explosion,
    Power_massUP,

    Power_firstIndex = 0,
    Power_lastIndex = Power_massUP,
    Power_numberOfPowers = Power_lastIndex + 1,
}

public class PowerUp : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters

    [SerializeField] PowerUpType powerType = PowerUpType.Power_firstIndex;
    [SerializeField] float powerUpActiveTime = 5f;
    [SerializeField] float powerUpStrength = 5f;
    [SerializeField] float powerUpReloadTime = 1f;
    [SerializeField] Color indicatorColor;


    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public PowerUpType GetPowerType()
    {
        if (powerType != PowerUpType.Power_numberOfPowers)
        {
            return powerType;
        }
        else
        {
            Debug.Log($"# Warning -> {gameObject.name} - PowerUpType == Power_numberOfPowers. Return LAST power up.");
            return PowerUpType.Power_lastIndex;
        }
    }

    public float GetPowerActiveTime()
    {
        return powerUpActiveTime;
    }

    public float GetPowerReloadTime()
    {
        return powerUpReloadTime;
    }

    public float GetPowerUpStrength()
    {
        return powerUpStrength;
    }

    public Color GetIndicatorColor()
    {
        return indicatorColor;
    }    

    #endregion
}
