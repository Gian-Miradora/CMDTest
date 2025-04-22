using UnityEngine;
using TMPro;

public class UI_EnemiesKilled : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake() => text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        GameManager.current.EnemiesKilledEvent += OnEnemiesKilled;
    }

    private void OnEnemiesKilled(int kill)
    {
        text.text = "Killed: " + kill;
    }
}
