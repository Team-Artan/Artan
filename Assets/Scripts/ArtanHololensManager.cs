using System.Collections.Generic;
using System.Collections.ObjectModel;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class ArtanHololensManager : Singleton<ArtanHololensManager> {
    private GazeManager gazeManager;
    private GestureRecognizer gestureRecognizer;
    private HandsManager handsManager;

    private Camera cam;

    private List<string> cmdList = new List<string>();

    private bool prevTapped = false;
    private Vector3 prevTapHandPosition;

    public bool Connected { get { return true; } }
    public bool Tapped { get; private set; }
    public bool Holding { get; private set; }
    public HololensTarget TargetObject { get; private set; }
    public Vector3 TapHandDeltaMove { get; private set; }
    public ReadOnlyCollection<string> CmdList { get { return new ReadOnlyCollection<string>(cmdList); } }

    public Vector3 GazePosition
    {
        get
        {
            return gazeManager.Position;
        }
    }

    private void Start()
    {
        gazeManager = GazeManager.Instance;

        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold);
        gestureRecognizer.TappedEvent += OnTap;
        gestureRecognizer.HoldStartedEvent += OnHoldBegin;
        gestureRecognizer.HoldCompletedEvent += OnHoldEnd;
        gestureRecognizer.HoldCanceledEvent += OnHoldEnd;

        gestureRecognizer.StartCapturingGestures();

        handsManager = HandsManager.Instance;
        prevTapHandPosition = new Vector3();
        TapHandDeltaMove = new Vector3();

        prevTapped = false;
        Tapped = false;
        Holding = false;

        cam = Camera.main;
    }

    private void Update()
    {
        if (prevTapped == false && Tapped == true) {
            prevTapped = true;
        }
        else if (prevTapped == true) {
            prevTapped = false;
            Tapped = false;
        }

        if (Holding == true) {
            TapHandDeltaMove = handsManager.ManipulationHandPosition - prevTapHandPosition;
            prevTapHandPosition = handsManager.ManipulationHandPosition;
        }

        Debug.DrawRay(cam.transform.position, GazePosition - cam.transform.position);
    }

    private void LateUpdate()
    {
        cmdList.Clear();
    }

    private void OnTap(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        Tapped = true;
        TargetObject = gazeManager.FocusedObject.GetComponent<HololensTarget>();
    }

    private void OnHoldBegin(InteractionSourceKind source, Ray headRay)
    {
        TargetObject = gazeManager.FocusedObject.GetComponent<HololensTarget>();
        if (TargetObject != null) {
            TargetObject.SetHeld(true);
        }

        Holding = true;
        prevTapHandPosition = handsManager.ManipulationHandPosition;
    }

    private void OnHoldEnd(InteractionSourceKind source, Ray headRay)
    {
        if (TargetObject != null) {
            TargetObject.SetHeld(false);
        }

        Holding = false;
        TargetObject = null;
        prevTapHandPosition.Set(0, 0, 0);
    }

    public void OnVoiceCommand(string cmd)
    {
        cmdList.Add(cmd);
    }
}