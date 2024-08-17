using System.Collections.Generic;
using UnityEngine;

public class PowerUpPool : MultipleObjectsPool
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    private List<PowerUp.TYPE> _availablePowerTypes;

    #endregion


    // -----------------------------------------------------------------------
    // Properties
    // -----------------------------------------------------------------------

    #region Properties

    public List<PowerUp.TYPE> AvailablePowerTypes => _availablePowerTypes;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public PowerUp RequestInactivePower(PowerUp.TYPE type, bool allowExtension = false)
    {
        PowerUp inactivePower = null;

        foreach (List<GameObject> objectsList in _pool)
        {
            PowerUp firstPower = objectsList[0].GetComponent<PowerUp>();

            if (firstPower.Stats.Type == type)
            {
                GameObject powerObject = FindInactiveObjectInList(objectsList);

                if (powerObject != null)
                {
                    inactivePower = powerObject.GetComponent<PowerUp>();
                }
                break;
            }
        }

        if ((inactivePower == null) && allowExtension)
        {
            inactivePower = ExtendPowersPool(type);
        }

        return inactivePower;
    }

    public void UpdateAvailablePowerTypes()
    {
        List<PowerUp.TYPE> availablePowerTypes = new();

        foreach (List<GameObject> objectsList in _pool)
        {
            PowerUp firstPower = objectsList[0].GetComponent<PowerUp>();
            availablePowerTypes.Add(firstPower.Stats.Type);
        }

        _availablePowerTypes = availablePowerTypes;
    }

    public void DeactivateAll()
    {
        foreach (List<GameObject> objectsList in _pool)
        {
            foreach (GameObject obj in objectsList)
            {
                obj.SetActive(false);
            }
        }
    }

    #endregion


    // -----------------------------------------------------------------------
    // Protected Methods
    // -----------------------------------------------------------------------

    #region Protected Methods

    protected override void Awake()
    {
        base.Awake();
        UpdateAvailablePowerTypes();
    }

    protected override bool FitsPushCondition(List<GameObject> sourceList, GameObject objToPush)
    {
        PowerUp firstListPower = sourceList[0].GetComponent<PowerUp>();
        PowerUp powerToPush = objToPush.GetComponent<PowerUp>();

        if (firstListPower.Stats.Type == powerToPush.Stats.Type)
        {
            return true;
        }

        return false;
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private PowerUp ExtendPowersPool(PowerUp.TYPE targetType)
    {
        PowerUp powerToCreate = null;

        foreach (PoolObjectData objectData in _dataList)
        {
            PowerUp power = objectData.Prefab.GetComponent<PowerUp>();

            if (power.Stats.Type == targetType)
            {
                powerToCreate = power;
                break;
            }
        }

        if (powerToCreate != null)
        {
            return CreateAndAddPower(powerToCreate, targetType);
        }
        else
        {
            string warningMsg = $"{targetType} is not supported in {nameof(_dataList)}, can not extend pool - {gameObject.name}.";
            ErrorsManager.Instance.SendWarningMsg(warningMsg);
        }

        return null;
    }

    private PowerUp CreateAndAddPower(PowerUp powerToCreate, PowerUp.TYPE targetType)
    {
        foreach (List<GameObject> objectsList in _pool)
        {
            PowerUp firstPower = objectsList[0].GetComponent<PowerUp>();

            if (firstPower.Stats.Type == targetType)
            {
                PowerUp createdPower = Instantiate(powerToCreate, firstPower.transform.parent);
                createdPower.name = firstPower.name;
                createdPower.gameObject.SetActive(false);

                objectsList.Add(createdPower.gameObject);

                return createdPower;
            }
        }
        return null;
    }

    #endregion
}
