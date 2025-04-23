using UnityEngine;
using TMPro;

public class UI_EnemiesKilled : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake() => text = GetComponent<TextMeshProUGUI>();

    private void OnEnable()
    {
        if (!GameManager.current) return;
        text.text = "Killed: " + GameManager.current.EnemiesKilled;
    }
}
