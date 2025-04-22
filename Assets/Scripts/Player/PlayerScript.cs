using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using UniRx;

[RequireComponent(typeof(Actor))]
public class PlayerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionAsset Inputs;
    [SerializeField] private GunState[] GunStates;

    private Actor actor;
    private Camera mainCamera;

    private InputAction move;
    private InputAction look;
    private InputAction fire;
    private InputAction changegun;

    private Vector2 moveDelta;
    private Vector2 lookDelta;

    private int currentGunState;
    public ActorState playerState { get; private set; }

    protected void SetState(ActorState state)
    {
        playerState = state;
        switch (playerState)
        {
            case ActorState.Move:
                GunStates[currentGunState].Move();
                break;
            case ActorState.Attack:
                break;
            case ActorState.Hit:
                GunStates[currentGunState].Hit();
                break;
            case ActorState.Death:
                GunStates[currentGunState].Death();
                break;
        }
    }

    private void Awake()
    {
        actor = GetComponent<Actor>();
        move = Inputs.FindAction("Move");
        look = Inputs.FindAction("Look");
        fire = Inputs.FindAction("Fire");
        //changegun = Inputs.FindAction("Switch Gun");
        //changegun.performed += ctx => ChangeGunState(ctx.ReadValue<float>() == 1f);

        fire.performed += ctx =>
        {
            if (playerState == ActorState.Death) return;
            GunStates[currentGunState].Shoot();
        };
    }

    private void Start()
    {
        GameManager.current.ObserveActorDamaged()
            .Where(x => x == actor)
            .Subscribe(_ => SetState(ActorState.Hit));

        GameManager.current.ObserveActorDeath()
            .Where(x => x == actor)
            .Subscribe(_ =>
            {
                Inputs.Disable();
                SetState(ActorState.Death);
            });

        GameManager.current.RoundStarted += Revive;
    }

    private void OnEnable()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        GunStates[currentGunState].SetGunState();
        Inputs.Enable();
    }

    private void OnDisable()
    {
        Inputs.Disable();
    }

    private void Update()
    {
        if (playerState == ActorState.Death) return;

        //get the value for input
        moveDelta = move.ReadValue<Vector2>();
        lookDelta = look.ReadValue<Vector2>();

        //Actor locomotion
        actor.MoveActor(moveDelta);
        actor.RotateActor(mainCamera.ScreenToWorldPoint(lookDelta), transform.right);
    }

    private void Revive()
    {
        SetState(ActorState.Move);
        transform.position = Vector3.zero;
        Inputs.Enable();
    }

    private void ChangeGunState(bool next)
    {
        if (next)
            currentGunState++;
        else
            currentGunState--;

        if (currentGunState >= GunStates.Length)
            currentGunState = 0;
        else if (currentGunState < 0)
            currentGunState = GunStates.Length - 1;

        GunStates[currentGunState].SetGunState();
    }

    private void ChangeGunState(int state)
    {
        if (state >= GunStates.Length) return;
        GunStates[currentGunState].SetGunState();
    }
}
