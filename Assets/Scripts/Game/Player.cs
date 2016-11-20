using System.Collections.Generic;
using UnityEngine;
public class Player {
    static private int nextID = 0;

    private Dictionary<int, Unit> unitList = new Dictionary<int, Unit>();

    public int ID { get; private set; }
    public Dictionary<int, Unit> UnitList { get { return new Dictionary<int, Unit>(unitList); } }
    public List<Unit> UnitArray { get { return new List<Unit>(unitList.Values); } }

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

    public Unit GetUnit(int index)
    {
        if (unitList.Count <= index) {
            return null;
        }

        return unitList[index];
    }

    public bool HasUnit(Unit unit)
    {
        return unitList.ContainsKey(unit.ID);
    }
}