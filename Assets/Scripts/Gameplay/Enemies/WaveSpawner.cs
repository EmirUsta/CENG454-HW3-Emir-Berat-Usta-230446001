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
    private bool _gameOver;

    void Awake()
    {
        var poolParent = new GameObject("EnemyPool").transform;
        _scoutPool = new ObjectPool<Enemy>(scoutPrefab, poolParent, 6);
        _brutePool = new ObjectPool<Enemy>(brutePrefab, poolParent, 3);
    }

    void OnEnable()
    {
        EventBus.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
        EventBus.OnGameStateChanged -= OnGameStateChanged;
    }

    public void StartWaves()
    {
        StartCoroutine(WaveRoutine());
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Win || state == GameState.Lose)
        {
            _gameOver = true;
            StopAllCoroutines();
        }
    }

    private IEnumerator WaveRoutine()
    {
        while (_currentWave < totalWaves && !_gameOver)
        {
            EventBus.WaveStarted(_currentWave + 1, totalWaves);
            yield return StartCoroutine(SpawnWave(_currentWave));
            _currentWave++;
            if (_currentWave < totalWaves)
                yield return new WaitForSeconds(timeBetweenWaves);
        }
        if (!_gameOver)
            EventBus.WaveCleared(_currentWave);
    }

    private IEnumerator SpawnWave(int waveIndex)
    {
        int scoutCount = 3 + waveIndex;
        int bruteCount = waveIndex / 2;

        for (int i = 0; i < scoutCount && !_gameOver; i++)
        {
            SpawnScout();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        for (int i = 0; i < bruteCount && !_gameOver; i++)
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
        e.Setup(new ZigzagStrategy(), coreTransform, 20f, 2f, () => _scoutPool.Return(e));
    }

    private void SpawnBrute()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Enemy e = _brutePool.Get();
        e.transform.position = point.position;
        e.Setup(new DirectChargeStrategy(), coreTransform, 60f, 1f, () => _brutePool.Return(e));
    }
}
