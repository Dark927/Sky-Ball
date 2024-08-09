using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [SerializeField] private List<PoolObjectData> _objectsDataList;

    private List<GameObject> _pool;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods
    public GameObject RequestInactiveObject(bool allowExpansion = false)
    {
        foreach (GameObject element in _pool)
        {
            if (!element.activeInHierarchy)
            {
                return element;
            }
        }


        // Create new element if all elements are active.

        if (allowExpansion)
        {
            if (_objectsDataList.Count != 0)
            {
                return AddNewElement(_objectsDataList[0].Prefab, transform.GetChild(0).transform);
            }
        }


        return null;
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _pool = new();
        InitPool();
    }

    private void InitPool()
    {
        foreach (PoolObjectData objectData in _objectsDataList)
        {
            GameObject objectsContainer = new(objectData.Title);
            objectsContainer.transform.parent = transform;

            for (int currentIndex = 0; currentIndex < objectData.CountToCreate; ++currentIndex)
            {
                AddNewElement(objectData.Prefab, objectsContainer.transform);
            }
        }
    }

    private GameObject AddNewElement(GameObject newElement, Transform parent = null)
    {
        if (parent == null)
        {
            parent = transform;
        }

        GameObject newObject = Instantiate(newElement, transform.position, Quaternion.identity, parent);
        newObject.SetActive(false);
        _pool.Add(newObject);

        return newObject;
    }

    #endregion

}
