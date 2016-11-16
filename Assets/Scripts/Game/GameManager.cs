using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    static public GameManager Instance { get; private set; }

    private List<Player> playerList = new List<Player>();

    private void Awake()
    {
        Instance = this;
    }

    public void DestroyUnit(Unit unit)
    {
        foreach (var player in playerList) {
            if (player.HasUnit(unit) == true) {
                player.RemoveUnit(unit);

                // TODO : Destroy effect
                Destroy(unit.gameObject);
            }
        }
    }
}