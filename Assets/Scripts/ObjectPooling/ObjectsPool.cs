using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [SerializeField] private PoolObjectData _objectData;

    private List<GameObject> _pool;
    private GameObject _container;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public GameObject RequestInactiveObject(bool allowExpansion = false)
    {
        if (_pool == null)
        {
            throw new NullReferenceException($"Pool is not initialized - {gameObject.name}.");
        }

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
            return AddNewElement(_objectData.Prefab, _container.transform);
        }

        return null;
    }

    public void DeactivateAll()
    {
        foreach (GameObject element in _pool)
        {
            element.SetActive(false);
        }
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        if (_objectData != null)
        {
            _pool = new();
            InitPool();
        }
        else
        {
            string errorMsg = $"{nameof(_objectData)} is null, can not init pool - {gameObject.name}.";
            ErrorsManager.Instance.SendErrorMsg(errorMsg);
        }
    }

    private void InitPool()
    {
        _container = new(_objectData.Title);
        _container.transform.parent = transform;

        for (int currentIndex = 0; currentIndex < _objectData.CountToCreate; ++currentIndex)
        {
            AddNewElement(_objectData.Prefab, _container.transform);
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
