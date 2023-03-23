using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour, IInteractable
{
    public InteractionChannel m_interactionChannel;

    private bool m_interactionState;

    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    void Start()
    {
        RegisterFunctions();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDestroy()
    {
        UnregisterFunctions();
    }

    void Update()
    {
        if (m_interactionState)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
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
