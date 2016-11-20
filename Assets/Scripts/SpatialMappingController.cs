using System;
using HoloToolkit.Unity;
using UnityEngine;

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
    }

    public void EndMapping()
    {
        smm.StopObserver();

        AstarPath.active.Scan();
    }
}