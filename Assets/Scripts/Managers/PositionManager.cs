using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    [Header("Main Settings")]
    [Space]

    public static PositionManager Instance;
    [SerializeField] private float _fallBoundY = -2f;


    [Space]
    [Header("Random position Settings")]
    [Space]

    [SerializeField] private float _safeDistance = 3f;
    [SerializeField] private float _spawnRadius = 8f;


    private float _errorBoundY = -50f;
    private Transform _playerTransform;


    public float DeathBoundY => _fallBoundY;
    public float ErrorBoundY => _errorBoundY;


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public Vector3 RandomSpawnPosition<Entity>() where Entity : MonoBehaviour
    {
        List<Vector3> entitiesPositions = GetActiveEntitiesPositions<Entity>();

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Fields for generating random spawn point
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        Vector3 actualSpawnPosition;
        bool isCorrectSpawnPoint;

        int maxAttempts = 100;


        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        // Generating position cycle 
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        do
        {
            Vector2 positionInCircle = Random.insideUnitCircle * _spawnRadius;
            actualSpawnPosition = new Vector3(positionInCircle.x, 0, positionInCircle.y);

            isCorrectSpawnPoint = true;
            maxAttempts--;

            // Check distance between generated position and each entity

            foreach (Vector3 entityPosition in entitiesPositions)
            {
                float distanceToEntity = (entityPosition - actualSpawnPosition).magnitude;

                if (distanceToEntity < _safeDistance)
                {
                    isCorrectSpawnPoint = false;
                    break;
                }
            }

        } while (!isCorrectSpawnPoint && (maxAttempts > 0));


        if (maxAttempts == 0)
        {
            actualSpawnPosition = new Vector3(0, _errorBoundY, 0);
        }

        return actualSpawnPosition;
    }

    public SpawnPoint FindFarthestSpawner(List<SpawnPoint> spawnersList)
    {
        SpawnPoint farthestSpawner = null;
        float sqrMaxDistance = 0;

        foreach (SpawnPoint spawner in spawnersList)
        {
            if (spawner.IsBlocked)
            {
                continue;
            }

            Vector3 spawnerPosition = spawner.transform.position;
            Vector3 playerPosition = _playerTransform.position;

            float sqrDistanceToPlayer = (playerPosition - spawnerPosition).sqrMagnitude;

            if (sqrDistanceToPlayer > sqrMaxDistance)
            {
                sqrMaxDistance = sqrDistanceToPlayer;
                farthestSpawner = spawner;
            }
        }

        return farthestSpawner;
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        SetInstance();

        PlayerMovement player = FindObjectOfType<PlayerMovement>();


        if (player != null)
        {
            _playerTransform = player.transform;
        }
        else
        {
            string errorMsg = $"{player} == null, can not find Player object! - {gameObject.name}";
            ErrorsManager.Instance.SendErrorMsg(errorMsg, true);
        }
    }

    private void SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    private List<Vector3> GetActiveEntitiesPositions<Entity>() where Entity : MonoBehaviour
    {
        List<Vector3> entitiesPositions = new() { _playerTransform.position };

        Entity[] activeEntities = FindObjectsOfType<Entity>();

        foreach (Entity entity in activeEntities)
        {
            entitiesPositions.Add(entity.transform.position);
        }

        return entitiesPositions;
    }

    #endregion
}
