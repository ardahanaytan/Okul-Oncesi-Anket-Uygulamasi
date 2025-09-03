// DoublePressConfirm.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class DoublePressConfirm : MonoBehaviour
{
    public UnityEvent OnConfirmed;   // 2. basışta çalışacak
    public float resetAfterSeconds = 2f; // 0 yaparsan süre kısıtı olmaz

    private Button _btn;
    private int _pressCount = 0;
    private float _lastPressTime = 0f;

    void Awake()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(HandleClick);
    }

    void OnDestroy()
    {
        if (_btn != null) _btn.onClick.RemoveListener(HandleClick);
    }

    void Update()
    {
        // Süre dolarsa sayacı sıfırla (istersen 0 yapıp tamamen kapat)
        if (resetAfterSeconds > 0f && _pressCount > 0 &&
            Time.unscaledTime - _lastPressTime > resetAfterSeconds)
        {
            _pressCount = 0;
        }
    }

    void OnDisable() => _pressCount = 0;

    private void HandleClick()
    {
        _lastPressTime = Time.unscaledTime;
        _pressCount++;

        if (_pressCount >= 2)
        {
            _pressCount = 0;
            OnConfirmed?.Invoke();
        }
    }
}
