using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Camera cam;
    public float speed = 5f; // Velocidad de movimiento del jugador
    private Animator animator;
    private Rigidbody rb;
    private PlayerInput plIn;
    private Vector2 moveInput;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        plIn = GetComponent<PlayerInput>();
        animator.SetBool("isDownning", false);
        animator.SetBool("isUpping", false);
        animator.SetBool("isLefting", false);
        animator.SetBool("isRighting", false);
        plIn.actions.FindActionMap("Combate").Disable();
        plIn.actions.FindActionMap("ControlMundo").Enable();
        plIn.actions.FindActionMap("Dialogo").Disable();
    }

    void Update()
    {
        // Movimiento antiguo guardado por si las moscas.
        /*float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical);*/

        // Obtener los valores de los ejes
        moveInput = plIn.actions["Movimiento"].ReadValue<Vector2>();

        // Crear el vector de movimiento
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        int moveX = (moveInput.x > 0) ? 1 : (moveInput.x < 0) ? -1 : 0;
        int moveY = (moveInput.y > 0) ? 1 : (moveInput.y < 0) ? -1 : 0;

        // Gestión de las animaciones con el movimiento
        switch (moveY)
        {
            case -1:
                animator.SetBool("isDownning", true);
                animator.SetBool("isUpping", false);

                switch (moveX)
                {
                    case -1:
                        animator.SetBool("isLefting", true);
                        animator.SetBool("isRighting", false);
                        break;
                    case 0:
                        animator.SetBool("isLefting", false);
                        animator.SetBool("isRighting", false);
                        break;
                    case 1:
                        animator.SetBool("isLefting", false);
                        animator.SetBool("isRighting", true);
                        break;
                }

                break;
            case 0:
                animator.SetBool("isDownning", false);
                animator.SetBool("isUpping", false);

                switch (moveInput.x)
                {
                    case -1:
                        animator.SetBool("isLefting", true);
                        animator.SetBool("isRighting", false);
                        break;
                    case 0:
                        animator.SetBool("isLefting", false);
                        animator.SetBool("isRighting", false);
                        break;
                    case 1:
                        animator.SetBool("isLefting", false);
                        animator.SetBool("isRighting", true);
                        break;
                }

                break;
            case 1:
                animator.SetBool("isDownning", false);
                animator.SetBool("isUpping", true);

                switch (moveInput.x)
                {
                    case -1:
                        animator.SetBool("isLefting", true);
                        animator.SetBool("isRighting", false);
                        break;
                    case 0:
                        animator.SetBool("isLefting", false);
                        animator.SetBool("isRighting", false);
                        break;
                    case 1:
                        animator.SetBool("isLefting", false);
                        animator.SetBool("isRighting", true);
                        break;
                }

                break;
        }

        // Normalizar el vector de movimiento si su magnitud es mayor que 1
        if (movement.magnitude > 1f)
        {
            movement = movement.normalized;
        }

        // Aplicar el movimiento con la velocidad y deltaTime
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // Movimiento de la cámara
        cam.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z - 4);
    }
}