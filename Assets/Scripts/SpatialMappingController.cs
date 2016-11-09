using UnityEngine;
using HoloToolkit.Unity;

[RequireComponent(typeof(SpatialMappingManager))]
public class SpatialMappingController : MonoBehaviour {
    private SpatialMappingManager smm;

    public Canvas canvas;
    public PositionGuide guide;

    private void Start()
    {
        smm = GetComponent<SpatialMappingManager>();
    }

    public void BeginMapping()
    {
        smm.StartObserver();
    }

    public void EndMapping()
    {
        smm.StopObserver();
        guide.gameObject.SetActive(true);
        canvas.gameObject.SetActive(true);
    }
}