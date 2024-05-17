using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    Controls controls;
    InputAction handBreak;
    private void Awake()
    {
        controls = new Controls();
        controls.Enable();
        controls.Movement.Enable();
    }


    void Update()
    {
        if (controls.Movement.HandBreak.IsPressed())
            print("a");
        else
            print("b");

        print(controls.Movement.Turn.ReadValue<float>());
    }
}
