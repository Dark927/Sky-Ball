using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    private EnemyDefaultHead _head;

    private void Awake()
    {
        _head = GetComponentInParent<EnemyDefaultHead>();
    }

    public void Die()
    {
        gameObject.SetActive(false);
        _head.Deactivate();
    }
}
