using UnityEngine;
using TMPro;

public class UI_TimeElapsed : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake() => text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        GameManager.current.TimeElapsedEvent += OnTimeElapsed;
    }

    private void OnTimeElapsed(float time)
    {
        text.text = "Time: " + time;
    }
}
