
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionProperty moveAction;
    [SerializeField] private InputActionProperty jumpAction;
    [SerializeField] private InputActionProperty fireAction;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;

    private Rigidbody2D rb;
    private bool jumpRequested;
    private bool enSuelo;
    public event System.Action<string> DispositivoUsado;


    private void DetectarDispositivo(InputAction.CallbackContext context)
    {
        string nombreDispositivo;

        if (context.control.device is Gamepad ||
            context.control.device.displayName.Contains("Controller") ||
            context.control.device.displayName.Contains("Gamepad"))
        {
            nombreDispositivo = "GAMEPAD";
        }
        else
        {
            nombreDispositivo = "TECLADO Y RATÓN";
        }

        Debug.Log(
            "Acción: " + context.action.name +
            " | Dispositivo: " + context.control.device.displayName +
            " | Tipo: " + nombreDispositivo
        );

        DispositivoUsado?.Invoke(nombreDispositivo);
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        moveAction.action?.Enable();
        jumpAction.action?.Enable();
        fireAction.action?.Enable();

        if (jumpAction.action != null)
            jumpAction.action.performed += OnJump;

        if (fireAction.action != null)
            fireAction.action.performed += OnFire;
        if (moveAction.action != null)
        {
            moveAction.action.performed += DetectarDispositivo;
            moveAction.action.canceled += DetectarDispositivo;
        }

        if (jumpAction.action != null)
        {
            jumpAction.action.performed += DetectarDispositivo;
        }

        if (fireAction.action != null)
        {
            fireAction.action.performed += DetectarDispositivo;
        }
    }

    private void OnDisable()
    {
        if (jumpAction.action != null)
            jumpAction.action.performed -= OnJump;

        if (fireAction.action != null)
            fireAction.action.performed -= OnFire;
        if (moveAction.action != null)
        {
            moveAction.action.performed -= DetectarDispositivo;
            moveAction.action.canceled -= DetectarDispositivo;
        }

        if (jumpAction.action != null)
        {
            jumpAction.action.performed -= DetectarDispositivo;
        }

        if (fireAction.action != null)
        {
            fireAction.action.performed -= DetectarDispositivo;
        }

        moveAction.action?.Disable();
        jumpAction.action?.Disable();
        fireAction.action?.Disable();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpRequested = true;
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("¡Disparo!");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }
    }
    private void FixedUpdate()
    {
        Vector2 movement = moveAction.action != null
            ? moveAction.action.ReadValue<Vector2>()
            : Vector2.zero;

        rb.linearVelocity = new Vector2(
            movement.x * moveSpeed,
            rb.linearVelocity.y
        );

        if (jumpRequested)
        {
            jumpRequested = false;

            if (enSuelo)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );

                enSuelo = false;
            }
        }
    }
}
