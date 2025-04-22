using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class GunState : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private string StateName;
    [SerializeField] private AudioClip bang;
    [SerializeField] private BulletPool pool;

    [Header("Parameters")]
    [SerializeField] private float DamageAmount;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float Cooldown;

    private Animator playerAnimator;
    private AudioSource audioSFXSource;

    private bool onCooldown;

    public void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        audioSFXSource = GetComponent<AudioSource>();
    }

    public void SetGunState() =>playerAnimator.SetBool(StateName, true);

    public async void Shoot()
    {
        if (onCooldown) return;
        onCooldown = await ShootTask();
    }

    public async Task<bool> ShootTask()
    {
        onCooldown = true;
        audioSFXSource.PlayOneShot(bang);

        var rayhit = new RaycastHit2D[1];
        if (Physics2D.RaycastNonAlloc(transform.position, transform.right, rayhit, Mathf.Infinity, collisionMask) > 0)
        {
            if (rayhit[0].transform.CompareTag("Enemy"))
            {
                var actor = rayhit[0].transform.gameObject.GetComponent<Actor>();
                actor.DamageActor(DamageAmount);
            }
        }

        await Task.Delay((int)(Cooldown * 1000f));
        return false;
    }

    public void Move() => playerAnimator.SetBool("Move", true);
    public void Hit() => playerAnimator.SetBool("Hit", true);

    public void Death() => playerAnimator.SetBool("Death", true);
}
