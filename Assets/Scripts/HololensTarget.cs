using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class HololensTarget : MonoBehaviour {
    private GestureManager gestureManager;

    private bool prevSelected;

    public bool IsGazing { get; private set; }
    public bool IsPressed { get; private set; }
    public bool IsManipulating { get; private set; }
    public bool IsSelected { get; private set; }

    private void Start()
    {
        prevSelected = false;

        IsGazing = false;
        IsPressed = false;
        IsManipulating = false;
        IsSelected = false;

        gestureManager = GestureManager.Instance;
        gestureManager.OnManipulationStarted += BeginManipulation;
        gestureManager.OnManipulationCompleted += EndManipulation;
        gestureManager.OnManipulationCanceled += EndManipulation;
    }

    private void Update()
    {
        // Selected in this frame
        if(prevSelected == false && IsSelected == true) {
            prevSelected = true;
        }
        // Selected in previous frame.  Deselect
        else if(prevSelected == true) {
            prevSelected = false;
            IsSelected = false;
        }
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

    private void BeginManipulation(InteractionSourceKind sourceKind)
    {
        IsManipulating = true;
    }

    private void EndManipulation(InteractionSourceKind sourceKind)
    {
        IsManipulating = false;
    }
}