using UnityEngine;
using TMPro;

public class HudController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coreHealthText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI resultText;

    private int _kills;

    void OnEnable()
    {
        EventBus.OnCoreDamaged += UpdateHealth;
        EventBus.OnEnemyDied += OnEnemyDied;
        EventBus.OnWaveCleared += OnWaveCleared;
        EventBus.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
        EventBus.OnCoreDamaged -= UpdateHealth;
        EventBus.OnEnemyDied -= OnEnemyDied;
        EventBus.OnWaveCleared -= OnWaveCleared;
        EventBus.OnGameStateChanged -= OnGameStateChanged;
    }

    void Start()
    {
        if (resultText != null) resultText.gameObject.SetActive(false);
        if (waveText != null) waveText.text = "Defend the Core";
    }

    private void UpdateHealth(float current, float max)
    {
        if (coreHealthText != null)
            coreHealthText.text = $"Core: {current:F0} / {max:F0}";
    }

    private void OnEnemyDied(Vector3 position)
    {
        _kills++;
        if (waveText != null) waveText.text = $"Kills: {_kills}";
    }

    private void OnWaveCleared(int wave)
    {
        if (waveText != null) waveText.text = $"All {wave} waves cleared";
    }

    private void OnGameStateChanged(GameState state)
    {
        if (resultText == null) return;
        if (state == GameState.Win)
        {
            resultText.text = "BREACH CONTAINED - YOU WIN";
            resultText.gameObject.SetActive(true);
        }
        else if (state == GameState.Lose)
        {
            resultText.text = "CORE LOST - GAME OVER";
            resultText.gameObject.SetActive(true);
        }
    }
}
