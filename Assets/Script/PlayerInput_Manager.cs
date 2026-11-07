using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput_Manager : MonoBehaviour
{
    public InputActionReference moveLeft;
    public InputActionReference moveRight;
    public InputActionReference jump;

    [HideInInspector] public int laneInput;
    [HideInInspector] public bool jumpInput;

    private void OnEnable()
    {
        moveLeft.action.Enable();
        moveRight.action.Enable();
        jump.action.Enable();
    }

    private void OnDisable()
    {
        moveLeft.action.Disable();
        moveRight.action.Disable();
        jump.action.Disable();
    }

    private void Update()
    {
        laneInput = 0;
        jumpInput = false;

        if (moveLeft.action.WasPressedThisFrame())
        {
            laneInput = -1;
        }
        else if (moveRight.action.WasPressedThisFrame())
        {
            laneInput = 1;
        }

        if (jump.action.WasPressedThisFrame())
        {
            jumpInput = true;
        }
    }
}
