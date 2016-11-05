using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class HololensTarget : MonoBehaviour {
    public bool IsGazing { get; private set; }
    public bool IsPressed { get; private set; }
    public bool IsHeld { get; private set; }
    public bool IsSelected { get; private set; }

    private void Start()
    {
        IsGazing = false;
        IsPressed = false;
        IsHeld = false;
        IsSelected = false;
    }

    private void LateUpdate()
    {
        IsSelected = false;
    }

    private void OnGazeEnter()
    {
        IsGazing = true;
    }

    private void OnGazeLeave()
    {
        IsGazing = false;
    }

    private void OnPressed()
    {
        IsPressed = true;
    }

    private void OnReleased()
    {
        IsPressed = false;
    }

    private void OnSelect()
    {
        IsSelected = true;
    }

    public void SetHeld(bool held)
    {
        IsHeld = held;
    }
}