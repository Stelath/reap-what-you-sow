using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;

    private PlayerControls playerControls;

    public bool inMainForm = true;
    public bool dropItem = false;

    private Vector2 movement;
    private Rigidbody2D rb;

    private SpriteRenderer spRenderer;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        spRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        ProcessInputs();
        Move();
    }

    private void Move()
    {
        rb.velocity = movement * speed;
        spRenderer.flipX = movement.x == 0 ? spRenderer.flipX : movement.x < 0;

        animator.SetBool("isWalking", movement.x != 0 || movement.y != 0);
        animator.SetBool("mainForm", inMainForm);
    }

    private void ProcessInputs()
    {
        movement = playerControls.Player.Move.ReadValue<Vector2>();
        inMainForm = playerControls.Player.ChangeForms.WasPressedThisFrame() ? !inMainForm : inMainForm;
        dropItem = playerControls.Player.Interact.IsPressed();

        if (dropItem){
            GetComponent<Holding>().Drop();
        }
    }
}
