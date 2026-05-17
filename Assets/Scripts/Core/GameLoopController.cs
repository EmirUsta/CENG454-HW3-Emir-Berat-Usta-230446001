using UnityEngine;

public class GameLoopController : MonoBehaviour
{
    [SerializeField] private WaveSpawner waveSpawner;

    private GameState _state = GameState.Intro;

    void OnEnable()
    {
        EventBus.OnCoreDestroyed += OnCoreDestroyed;
        EventBus.OnWaveCleared += OnWaveCleared;
    }

    void OnDisable()
    {
        EventBus.OnCoreDestroyed -= OnCoreDestroyed;
        EventBus.OnWaveCleared -= OnWaveCleared;
    }

    void Start()
    {
        TransitionTo(GameState.Playing);
        waveSpawner.StartWaves();
    }

    private void TransitionTo(GameState newState)
    {
        _state = newState;
        EventBus.GameStateChanged(newState);
    }

    private void OnCoreDestroyed() => TransitionTo(GameState.Lose);
    private void OnWaveCleared(int wave) => TransitionTo(GameState.Win);
}
