using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Enemy scoutPrefab;
    [SerializeField] private Enemy brutePrefab;
    [SerializeField] private Transform coreTransform;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int totalWaves = 5;
    [SerializeField] private float timeBetweenWaves = 8f;
    [SerializeField] private float timeBetweenSpawns = 0.5f;

    private ObjectPool<Enemy> _scoutPool;
    private ObjectPool<Enemy> _brutePool;
    private int _currentWave;

    void Awake()
    {
        var poolParent = new GameObject("EnemyPool").transform;
        _scoutPool = new ObjectPool<Enemy>(scoutPrefab, poolParent, 6);
        _brutePool = new ObjectPool<Enemy>(brutePrefab, poolParent, 3);
    }

    public void StartWaves()
    {
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        while (_currentWave < totalWaves)
        {
            yield return StartCoroutine(SpawnWave(_currentWave));
            _currentWave++;
            if (_currentWave < totalWaves)
                yield return new WaitForSeconds(timeBetweenWaves);
        }
        EventBus.WaveCleared(_currentWave);
    }

    private IEnumerator SpawnWave(int waveIndex)
    {
        int scoutCount = 3 + waveIndex;
        int bruteCount = waveIndex / 2;

        for (int i = 0; i < scoutCount; i++)
        {
            SpawnScout();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        for (int i = 0; i < bruteCount; i++)
        {
            SpawnBrute();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private void SpawnScout()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Enemy e = _scoutPool.Get();
        e.transform.position = point.position;
        e.Setup(new ZigzagStrategy(), coreTransform, 20f, 3f, () => _scoutPool.Return(e));
    }

    private void SpawnBrute()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Enemy e = _brutePool.Get();
        e.transform.position = point.position;
        e.Setup(new DirectChargeStrategy(), coreTransform, 60f, 1.5f, () => _brutePool.Return(e));
    }
}
