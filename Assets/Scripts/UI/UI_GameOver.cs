using UnityEngine;
using UniRx;

public class UI_GameOver : MonoBehaviour
{
    private void Start()
    {
        GameManager.current.ObserveActorDeath()
            .Where(x => x.gameObject.CompareTag("Player"))
            .Subscribe(_ => gameObject.SetActive(true));

        gameObject.SetActive(false);
    }
}
