using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    [HideInInspector] public PlayerInputs controls;
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public bool mainAttackKey;
    [HideInInspector] public PlayerInputs interact;

    private void Awake()
    {
        
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

        controls = new PlayerInputs();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();

        //Attack key
        controls.Player.MainAttack.performed += ctx => mainAttackKey = true;
        controls.Player.MainAttack.canceled += ctx => mainAttackKey = false;

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
