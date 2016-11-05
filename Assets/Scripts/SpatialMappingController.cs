using UnityEngine;
using HoloToolkit.Unity;

[RequireComponent(typeof(SpatialMappingManager))]
public class SpatialMappingController : MonoBehaviour {
    private SpatialMappingManager smm;

    private void Start()
    {
        smm = GetComponent<SpatialMappingManager>();
    }

    public void BeginMapping()
    {
        smm.StartObserver();
        Debug.Log("Begin Mapping");
    }

    public void EndMapping()
    {
        smm.StopObserver();
        Debug.Log("End Mapping");
    }
}