using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MultipleObjectsPool : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [SerializeField] protected List<PoolObjectData> _dataList;
    protected List<List<GameObject>> _pool;

    #endregion


    // -----------------------------------------------------------------------
    // Properties
    // -----------------------------------------------------------------------

    #region Properties

    public int ActiveObjectsCount
    {
        get
        {
            int count = 0;

            foreach (List<GameObject> objectsList in _pool)
            {
                foreach (GameObject obj in objectsList)
                {
                    if (obj.gameObject.activeInHierarchy)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public GameObject RequestFirstInactiveObject()
    {
        foreach (List<GameObject> objectsList in _pool)
        {
            foreach (GameObject obj in objectsList)
            {
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }
        }
        return null;
    }

    #endregion


    // -----------------------------------------------------------------------
    // Protected Methods
    // -----------------------------------------------------------------------

    #region Protected Methods

    protected abstract bool FitsPushCondition(List<GameObject> sourceList, GameObject objToPush);

    protected void PushToPool(GameObject objToPush)
    {
        bool pushed = false;

        // Check if a list with the same type of enemy exists

        for (int currentListIndex = 0; currentListIndex < _pool.Count; ++currentListIndex)
        {
            if (FitsPushCondition(_pool[currentListIndex], objToPush))
            {
                _pool[currentListIndex].Add(objToPush);
                pushed = true;
                break;
            }
        }

        // Create a new list if it does not already exist

        if (!pushed)
        {
            List<GameObject> newList = new() { objToPush };
            _pool.Add(newList);
        }
    }

    protected virtual void Awake()
    {
        _pool = new();
        InitPool();
    }

    protected void InitPool()
    {
        foreach (PoolObjectData objectData in _dataList)
        {
            GameObject objectsContainer = new(objectData.Title);
            objectsContainer.transform.parent = transform;

            for (int currentIndex = 0; currentIndex < objectData.CountToCreate; ++currentIndex)
            {
                GameObject obj = objectData.Prefab;

                if (obj != null)
                {
                    AddNewObject(obj, objectsContainer.transform);
                }
                else
                {
                    string errorMsg = $"{nameof(objectData)}.Prefab is null!";
                    ErrorsManager.Instance.SendErrorMsg(errorMsg);
                }
            }
        }
    }

    protected void AddNewObject(GameObject objToAdd, Transform parent = null)
    {
        if (parent == null)
        {
            parent = transform;
        }

        GameObject newObject = Instantiate(objToAdd, transform.position, Quaternion.identity, parent);
        newObject.name = objToAdd.name;
        newObject.SetActive(false);

        PushToPool(newObject);
    }

    protected GameObject FindInactiveObjectInList(List<GameObject> objList)
    {
        foreach (GameObject obj in objList)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods



    #endregion

}