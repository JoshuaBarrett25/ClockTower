using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance { get; private set; }

    [field: SerializeField] public SpawnPoint currentSpawnPoint { get; private set; }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetNewSpawnPoint(SpawnPoint newSpawnPoint)
    {
        currentSpawnPoint = newSpawnPoint;
    }

    public Transform GetSpawnPoint()
    {
        return currentSpawnPoint.transform;
    }
}
