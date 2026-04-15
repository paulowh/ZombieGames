using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// Controla o movimento, a visão (mouse) e as ações de combate em primeira pessoa (FPS).
/// Utiliza o CharacterController para física e o novo Input System para entradas.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController_FPS : MonoBehaviour
{
    #region Configurações e Variáveis
    [Header("Sistema de Combate")]
    [Tooltip("Evento disparado ao pressionar o botão de ataque (disparo).")]
    public UnityEvent OnShoot;

    [Tooltip("Evento disparado ao receber dano de zumbis ou perigos.")]
    public UnityEvent OnTakeDamage;

    [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float gravity = -20.0f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Configurações de Visão (Mouse)")]
    [Tooltip("Objeto que a Cinemachine Camera seguirá (representa a cabeça).")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float mouseSensitivity = 0.1f;

    // Variáveis de Estado Internas
    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    private InputAction fireAction;

    private Vector3 verticalVelocity;
    private float verticalRotation = 0f; // Armazena a inclinação da cabeça (cima/baixo)
    #endregion

    #region Inicialização
    /// <summary>
    /// Inicializa referências e configura o estado do cursor do sistema.
    /// </summary>
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        // Mapeamento das ações baseadas no Input Action Asset
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        lookAction = playerInput.actions["Look"];
        fireAction = playerInput.actions["Attack"];

        // Trava o mouse no centro da tela e o esconde para jogabilidade FPS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    #endregion

    #region Loop Principal
    /// <summary>
    /// Ordem de execução: Visão -> Movimento -> Combate.
    /// </summary>
    void Update()
    {
        HandleLook();
        HandleMovement();
        HandleCombatInput();
    }
    #endregion

    #region Métodos de Controle de Visão
    /// <summary>
    /// Processa o movimento do mouse para girar o corpo (horizontal) e a cabeça (vertical).
    /// </summary>
    private void HandleLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        // ROTAÇÃO HORIZONTAL (Corpo):
        // Ainda precisamos disso para que o personagem gire para os lados
        // e o 'transform.forward' funcione corretamente no movimento.
        transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity);

        // ROTAÇÃO VERTICAL (Cabeça):
        // COMENTADO: Agora o Cinemachine Pan Tilt cuida disso automaticamente!
        /*
        verticalRotation -= lookInput.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTarget.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        */
    }
    #endregion

    #region Métodos de Movimentação Física
    /// <summary>
    /// Calcula o movimento horizontal relativo ao olhar e a física vertical (pulo/gravidade).
    /// </summary>
    private void HandleMovement()
    {
        // Verifica se o CharacterController está tocando o chão
        bool isGrounded = controller.isGrounded;

        // --- Lógica de Gravidade e Pulo ---
        if (isGrounded && verticalVelocity.y < 0)
        {
            // Aplica uma pequena força constante para baixo para manter o estado Grounded estável
            verticalVelocity.y = -2f;
        }

        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            // Fórmula física de pulo: RaizQuadrada(Altura * -2 * Gravidade)
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplica a aceleração da gravidade ao longo do tempo
        verticalVelocity.y += gravity * Time.deltaTime;

        // --- Lógica de Movimento Horizontal ---
        Vector2 input = moveAction.ReadValue<Vector2>();

        // Transformamos o input local (WASD) em direções relativas para onde o Player está olhando
        // transform.forward é a direção do "nariz" do personagem no mundo
        Vector3 moveDirection = (transform.forward * input.y) + (transform.right * input.x);
        //moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        //moveDirection = transform.TransformDirection(moveDirection);

        // --- Consolidação Final ---
        // Combinamos a velocidade de movimento com a velocidade vertical
        Vector3 finalVelocity = (moveDirection.normalized * moveSpeed) + verticalVelocity;

        // Executa o movimento via CharacterController uma única vez no frame
        controller.Move(finalVelocity * Time.deltaTime);
    }
    #endregion

    #region Métodos de Combate e Dano
    /// <summary>
    /// Verifica a entrada de ataque e dispara o evento OnShoot.
    /// </summary>
    private void HandleCombatInput()
    {
        if (fireAction.triggered)
        {
            OnShoot?.Invoke();
        }
    }

    /// <summary>
    /// Método público para processar dano recebido.
    /// </summary>
    /// <param name="amount">Quantidade de dano.</param>
    public void TakeDamage(float amount)
    {
        Debug.Log("Dano recebido no FPS: " + amount);
        OnTakeDamage?.Invoke();
    }
    #endregion
}