using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class ArtanHololensManager : Singleton<ArtanHololensManager> {
    private GazeManager gazeManager;
    private GestureRecognizer gestureRecognizer;
    private Camera cam;

    private bool prevTapped = false;

    public bool Connected { get { return true; } }
    public bool Tapped { get; private set; }

    public Vector3 GazePosition
    {
        get
        {
            var dir = gazeManager.Position - cam.transform.position;
            RaycastHit hit;
            var result = Physics.Raycast(cam.transform.position, dir, out hit, Mathf.Infinity);
            if (result == true) {
                return hit.point;
            }
            else {
                return new Vector3();
            }
        }
    }

    private void Start()
    {
        gazeManager = GazeManager.Instance;

        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold);
        gestureRecognizer.TappedEvent += OnTap;

        gestureRecognizer.StartCapturingGestures();

        prevTapped = false;
        Tapped = false;

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

        Debug.DrawRay(cam.transform.position, GazePosition - cam.transform.position);
    }

    private void OnTap(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        Tapped = true;
    }
}
