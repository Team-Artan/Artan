using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    enum GameState {
        Begin,
        Scanning,
        UnitPlacing,
        Playing
    }

    enum TurnOwner {
        Human,
        Cpu
    }

    static public GameManager Instance { get; private set; }

    public int InitialUnitCount = 5;

    private ArtanHololensManager hm;
    private SpatialMappingController smm;

    private GameState state = GameState.Begin;
    private TurnOwner turnOwner = TurnOwner.Human;

    private List<Player> playerList = new List<Player>();
    private Player human;
    private Player cpu;

    // Unit place
    private Unit unitPrefab;
    private PositionGuide guideTankPrefab;
    private Canvas guideCanvasPrefab;

    private PositionGuide unitGuide;
    private int curUnitPlaced = 0;

    // Turn
    private Unit curUnit;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        hm = ArtanHololensManager.Instance;
        smm = FindObjectOfType<SpatialMappingController>();

        unitPrefab = (Resources.Load("Prefabs/PlayerTank") as GameObject).GetComponent<Unit>();
        guideTankPrefab = (Resources.Load("Prefabs/GuideTank") as GameObject).GetComponent<PositionGuide>();
        guideCanvasPrefab = (Resources.Load("Prefabs/GuideCanvas") as GameObject).GetComponent<Canvas>();
    }

    private void Update()
    {
        if (hm == null) {
            return;
        }

        switch (state) {
            case GameState.Begin: {
                    if (hm.Tapped == true || hm.GetVoiceCommand("begin") == true) {
                        BeginScanning();
                    }
                }
                break;

            case GameState.Scanning: {
                    if (hm.Tapped == true || hm.GetVoiceCommand("finish") == true) {
                        BeginUnitPlacing();
                    }
                }
                break;

            case GameState.UnitPlacing: {
                    // guide.tapCallback is called
                }
                break;

            case GameState.Playing: {
                    switch (turnOwner) {
                        case TurnOwner.Human: {
                                StartCoroutine(HumanTurn());
                            }
                            break;

                        case TurnOwner.Cpu: {
                                StartCoroutine(CpuTurn());
                            }
                            break;
                    }
                }
                break;
        }
    }

    private void ChangeGameState(GameState state)
    {
        if (this.state == state) {
            return;
        }

        this.state = state;

        Debug.Log("State change : " + state.ToString());
    }

    private IEnumerator HumanTurn()
    {
        var unitList = human.UnitList;

        foreach (var pair in unitList) {
            var unit = pair.Value;
            curUnit = unit;

            var control = unit.GetComponent<TankController>();
            control.BeginTurn();

            while (control.TurnEnded == false) {
                control.HandleInput();
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
        }

        // Turn end
        turnOwner = TurnOwner.Cpu;
        curUnit = null;

        yield return null;
    }

    private IEnumerator CpuTurn()
    {
        var unitList = human.UnitList;

        foreach (var pair in unitList) {
            var unit = pair.Value;
            curUnit = unit;

            yield return new WaitForEndOfFrame();
        }

        // Turn end
        turnOwner = TurnOwner.Human;
        curUnit = null;

        yield return null;
    }

    public void BeginScanning()
    {
        smm.BeginMapping();

        ChangeGameState(GameState.Scanning);
    }

    public void BeginUnitPlacing()
    {
        // 2 players for now, one is human and one is cpu
        human = new Player();
        cpu = new Player();

        playerList.Add(human);
        playerList.Add(cpu);

        foreach (var player in playerList) {
            for (int i = 0; i < InitialUnitCount; ++i) {
                var unit = Instantiate(unitPrefab).GetComponent<Unit>();
                unit.gameObject.SetActive(false);

                player.AddUnit(unit);
            }
        }

        foreach (var pair in cpu.UnitList) {
            pair.Value.gameObject.SetActive(true);
        }

        unitGuide = Instantiate(guideTankPrefab).GetComponent<PositionGuide>();
        unitGuide.tapCallback = (pos) => {
            var unit = human.UnitArray[curUnitPlaced];
            unit.gameObject.SetActive(true);
            unit.SetPosition(pos);

            ++curUnitPlaced;

            if (curUnitPlaced == InitialUnitCount) {
                ChangeGameState(GameState.Playing);

                Destroy(unitGuide.gameObject);
            }
        };

        ChangeGameState(GameState.UnitPlacing);
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