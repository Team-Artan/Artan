using UnityEngine;

public class Unit : MonoBehaviour {
    public int HP = 1000;
    public int AP = 100;
    public int Atk = 100;

    public bool IsDead { get; private set; }

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
}