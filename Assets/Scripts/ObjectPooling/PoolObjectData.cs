using UnityEngine;

[CreateAssetMenu(fileName = "PoolObjectData")]
public class PoolObjectData : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _countToCreate;

    public string Title => _title;
    public GameObject Prefab => _prefab;
    public int CountToCreate => _countToCreate;
}
