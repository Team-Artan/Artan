using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour {
    static private int nextID = 0;

    public int ID { get; private set; }

    private Dictionary<int, Unit> unitList = new Dictionary<int, Unit>();

    private void Awake()
    {
        ID = nextID++;
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit.ID, unit);
    }

    public void RemoveUnit(Unit unit)
    {
        if (unitList.ContainsKey(unit.ID) == true) {
            unitList.Remove(unit.ID);
        }
    }

    public bool HasUnit(Unit unit)
    {
        return unitList.ContainsKey(unit.ID);
    }
}