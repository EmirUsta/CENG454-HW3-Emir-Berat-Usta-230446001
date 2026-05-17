using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioCueListener : MonoBehaviour
{
    [SerializeField] private AudioClip coreDamagedClip;
    [SerializeField] private AudioClip coreDestroyedClip;
    [SerializeField] private AudioClip enemyDiedClip;

    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        EventBus.OnCoreDamaged += OnCoreDamaged;
        EventBus.OnCoreDestroyed += OnCoreDestroyed;
        EventBus.OnEnemyDied += OnEnemyDied;
    }

    void OnDisable()
    {
        EventBus.OnCoreDamaged -= OnCoreDamaged;
        EventBus.OnCoreDestroyed -= OnCoreDestroyed;
        EventBus.OnEnemyDied -= OnEnemyDied;
    }

    private void OnCoreDamaged(float current, float max) => PlayCue(coreDamagedClip);
    private void OnCoreDestroyed() => PlayCue(coreDestroyedClip);
    private void OnEnemyDied(Vector3 position) => PlayCue(enemyDiedClip);

    private void PlayCue(AudioClip clip)
    {
        if (clip != null)
            _audioSource.PlayOneShot(clip);
    }
}
