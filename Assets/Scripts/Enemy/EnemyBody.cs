using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    private EnemyHead _head;

    private void Awake()
    {
        _head = GetComponentInParent<EnemyHead>();
    }

    public void Die()
    {
        gameObject.SetActive(false);
        _head.Deactivate();
    }
}
