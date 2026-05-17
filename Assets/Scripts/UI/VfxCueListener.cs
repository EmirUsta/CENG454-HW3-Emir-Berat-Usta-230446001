using System.Collections;
using UnityEngine;

public class VfxCueListener : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Color damageFlash = new Color(0.6f, 0.1f, 0.1f);
    [SerializeField] private float flashDuration = 0.12f;

    private Color _baseColor;
    private Coroutine _flashRoutine;

    void Awake()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        _baseColor = targetCamera.backgroundColor;
    }

    void OnEnable()
    {
        EventBus.OnCoreDamaged += OnCoreDamaged;
        EventBus.OnCoreDestroyed += OnCoreDestroyed;
    }

    void OnDisable()
    {
        EventBus.OnCoreDamaged -= OnCoreDamaged;
        EventBus.OnCoreDestroyed -= OnCoreDestroyed;
    }

    private void OnCoreDamaged(float current, float max) => Flash(damageFlash);
    private void OnCoreDestroyed() => Flash(Color.red);

    private void Flash(Color color)
    {
        if (_flashRoutine != null) StopCoroutine(_flashRoutine);
        _flashRoutine = StartCoroutine(FlashRoutine(color));
    }

    private IEnumerator FlashRoutine(Color color)
    {
        targetCamera.backgroundColor = color;
        yield return new WaitForSeconds(flashDuration);
        targetCamera.backgroundColor = _baseColor;
    }
}
