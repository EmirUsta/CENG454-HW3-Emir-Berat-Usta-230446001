using System;
using UnityEngine;

public static class EventBus
{
    public static event Action<float, float> OnCoreDamaged;
    public static event Action<float> OnCoreShieldChanged;
    public static event Action OnCoreDestroyed;
    public static event Action<Vector3> OnEnemyDied;
    public static event Action<int> OnWaveCleared;
    public static event Action<GameState> OnGameStateChanged;

    public static void CoreDamaged(float current, float max) => OnCoreDamaged?.Invoke(current, max);
    public static void CoreShieldChanged(float shield) => OnCoreShieldChanged?.Invoke(shield);
    public static void CoreDestroyed() => OnCoreDestroyed?.Invoke();
    public static void EnemyDied(Vector3 position) => OnEnemyDied?.Invoke(position);
    public static void WaveCleared(int waveIndex) => OnWaveCleared?.Invoke(waveIndex);
    public static void GameStateChanged(GameState state) => OnGameStateChanged?.Invoke(state);
}
