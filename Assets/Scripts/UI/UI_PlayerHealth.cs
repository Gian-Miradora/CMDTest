using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHealth : MonoBehaviour
{
    private Slider playerHealthSlider;

    private Coroutine sliderRoutine;
    public void Start()
    {
        playerHealthSlider = GetComponent<Slider>();

        GameManager.current.ObserveActorDamaged()
            .Where(x => x.gameObject.CompareTag("Player"))
            .Subscribe(x => UpdateSlider(x));

        GameManager.current.ObserveActorDeath()
            .Where(x => x.gameObject.CompareTag("Player"))
            .Subscribe(x => UpdateSlider(x));

        GameManager.current.RoundStarted += () => playerHealthSlider.value = playerHealthSlider.maxValue;
    }

    public void UpdateSlider(Actor player = null)
    {
        //playerHealthSlider.value = player.currentHealth;

        if (sliderRoutine != null)
            StopCoroutine(sliderRoutine);

        sliderRoutine = StartCoroutine(SmoothUpdate(player.currentHealth));
    }

    private IEnumerator SmoothUpdate(float endValue)
    {
        float speed = 3f;
        float elapsedTime = 0f;
        do
        {
            elapsedTime += Time.deltaTime;
            playerHealthSlider.value = Mathf.Lerp(playerHealthSlider.value, endValue, elapsedTime * speed);
            yield return new WaitForEndOfFrame();
        } while (elapsedTime < 1f);
        playerHealthSlider.value = endValue;
        yield return null;
    }
}
