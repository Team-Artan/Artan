using UnityEngine;
using HoloToolkit.Unity;

[RequireComponent(typeof(SpatialMappingManager))]
public class SpatialMappingController : MonoBehaviour {
    private SpatialMappingManager smm;

    public Unit unitPrefab;
    public PositionGuide guideTankPrefab;
    public Canvas guideCanvasPrefab;

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

        var guide = (Instantiate(guideTankPrefab.gameObject) as GameObject).GetComponent<PositionGuide>();
        var canvas = Instantiate(guideCanvasPrefab.gameObject) as GameObject;

        guide.tapCallback = (gazePosition) => {
            var unit = Instantiate(unitPrefab.gameObject) as GameObject;

            var obj = Instantiate(unit, gazePosition, Quaternion.identity);
            obj.SetActive(true);
            obj.transform.position = gazePosition;

            Destroy(canvas);
            Destroy(guide.gameObject);
        };

        AstarPath.active.Scan();
    }
}