using UnityEngine;

public class Unit : MonoBehaviour {
    static private int nextID = 0;

    public int HP = 1000;
    public int AP = 100;
    public int Atk = 100;

    public int ID { get; private set; }
    public bool IsDead { get; private set; }

    private void Awake()
    {
        ID = ++nextID;
    }

    private void Start()
    {
        IsDead = false;
    }

    private void Upate()
    {
    }

    public void Attack(Unit enemy)
    {
        enemy.Damage(Atk);
    }

    public void Damage(int damage)
    {
        HP -= damage;

        if (HP <= 0) {
            IsDead = true;
        }
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}