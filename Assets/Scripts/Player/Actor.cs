using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum ActorState { Move, Attack, Hit, Death }

public class Actor : MonoBehaviour
{
    [SerializeField] private GameObject Character;
    [SerializeField] private float Health;
    [SerializeField] private float MoveSpeed;

    private Rigidbody2D rb2D;

    public bool invulnerable { get; private set; }
    public float currentHealth { get; private set; }

    private void Awake() => rb2D = GetComponent<Rigidbody2D>();
    private void Start()
    {
        GameManager.current.RoundStarted += () => currentHealth = Health;
    }
    private void OnEnable() => currentHealth = Health;

    public void MoveActor(Vector2 moveDelta)
    {
        rb2D.velocity = moveDelta * MoveSpeed;
    }

    public void RotateActor(Vector3 target, Vector3 forward)
    {
        var newAngle = -Vector3.SignedAngle(new Vector3(target.x, target.y) - transform.position, forward, Vector3.forward);
        Character.transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }

    public void DamageActor(float damageAmount)
    {
        if (invulnerable || currentHealth <= 0) return;
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, Health);

        Debug.Log($"{gameObject.name} Health: {currentHealth}/{Health}");

        if (currentHealth <= 0)
        {
            GameManager.current.ActorDied.OnNext(this);
        }
        else
        {
            GameManager.current.ActorDamaged.OnNext(this);
        }
    }

    public void SetInvulnerability(bool inv) => invulnerable = inv;
}
