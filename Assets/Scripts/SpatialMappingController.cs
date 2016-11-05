using UnityEngine;
using HoloToolkit.Unity;

[RequireComponent(typeof(SpatialMappingManager))]
public class SpatialMappingController : MonoBehaviour {
    private SpatialMappingManager smm;

    private void Start()
    {
        smm = GetComponent<SpatialMappingManager>();
    }
}