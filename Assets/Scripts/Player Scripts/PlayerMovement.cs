using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IInteractable
{
    public InteractionChannel m_interactionChannel;

    private bool m_interactionState;

    public float moveSpeed;
    public float drag;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        RegisterFunctions();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void OnDestroy()
    {
        UnregisterFunctions();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl();
        rb.drag = drag;
    }

    private void FixedUpdate()
    {
        if (!m_interactionState)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public void RegisterFunctions()
    {
        m_interactionChannel.OnInteract += OnInteract;
        m_interactionChannel.OnEndInteraction += OnEndInteraction;
        m_interactionChannel.OnLookedAt += OnLookedAt;
        m_interactionChannel.OnLookAway += OnLookAway;
    }

    public void UnregisterFunctions()
    {
        m_interactionChannel.OnInteract -= OnInteract;
        m_interactionChannel.OnEndInteraction -= OnEndInteraction;
        m_interactionChannel.OnLookedAt -= OnLookedAt;
        m_interactionChannel.OnLookAway -= OnLookAway;
    }

    public void OnLookedAt(GameObject gameObject)
    {
        return;
    }

    public void OnLookAway(GameObject gameObject)
    {
        return;
    }

    public void OnInteract(GameObject gameObject)
    {
        m_interactionState = true;
    }

    public void OnEndInteraction(GameObject gameObject)
    {
        m_interactionState = false;
    }
}
