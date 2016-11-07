using UnityEngine;
using HoloToolkit.Unity;

[RequireComponent(typeof(SpatialMappingManager))]
public class SpatialMappingController : MonoBehaviour {
    private SpatialMappingManager smm;
    private Pathfinder pf;

    public Canvas canvas;
    public PositionGuide guide;

    public Pathfinder Pf { get { return pf; } }

    private void Start()
    {
        smm = GetComponent<SpatialMappingManager>();
        pf = Pathfinder.Instance;
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

        pf.CreateMap();
    }
}