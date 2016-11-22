using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Health_UI : MonoBehaviour {

    public GameObject targetTank;

    public GameObject HPSurface;
    public GameObject HP_Bar;

    private Unit unit;

    //현재 상태를 알려줌. Move 와 Stop
    string cur_state;
    // Use this for initialization
    void Start()
    {
        HPSurface = GameObject.Find("HP_Surface");
        HP_Bar = GameObject.Find("HP_Bar");
        cur_state = "Stop";
        unit = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(targetTank.transform.position.x, targetTank.transform.position.y, targetTank.transform.position.z);
        ChangeUI();

        //StartCoroutine(move_damp(1f));
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    if(cur_state == "Move")
        //    {
        //        cur_state = "Stop";
        //    }
        //    else
        //    {
        //        cur_state = "Move";
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    GetDamaged(20);
        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    GetHeal(20);
        //}
    }

    public void GetDamaged(float damage)
    {
        StopAllCoroutines();
        StartCoroutine(vibrate());
        StartCoroutine(col_dam());

        int maxHP = unit.MaxHP;
        int curHP = unit.HP;

        if (HP_Bar.GetComponent<Image>().fillAmount > 0) {
            if (HP_Bar.GetComponent<Image>().fillAmount - damage / maxHP >= 0) {
                //HP_Bar.GetComponent<Image>().fillAmount -= damage / fullHP;
                StartCoroutine(hp_damp(HP_Bar.GetComponent<Image>().fillAmount - damage / maxHP));

            }
            else {
                //HP_Bar.GetComponent<Image>().fillAmount = 0;
                StartCoroutine(hp_damp(0));
            }
        }
    }
    public void GetHeal(float heal)
    {
        int maxHP = unit.MaxHP;

        if (HP_Bar.GetComponent<Image>().fillAmount < 1) {
            if (HP_Bar.GetComponent<Image>().fillAmount + heal / maxHP <= 1) {
                StartCoroutine(hp_damp(HP_Bar.GetComponent<Image>().fillAmount + heal / maxHP));
            }
            else {
                StartCoroutine(hp_damp(1));
            }
        }
    }
    public void ChangeUI()
    {
        if (cur_state == "Move") {
            StartCoroutine(MoveUI(0.4f));
            StartCoroutine(HealthUI(0));
        }
        else if (cur_state == "Stop") {
            StartCoroutine(MoveUI(0));
            StartCoroutine(HealthUI(0.4f));
        }
    }
    public void changeStatus()
    {
        if (cur_state == "Move") {
            cur_state = "Stop";
        }
        else {
            cur_state = "Move";
        }
    }

    IEnumerator HealthUI(float alpha)
    {
        Image hpbar = HP_Bar.GetComponent<Image>();
        if (hpbar.color.a < alpha) {
            while (hpbar.color.a < alpha) {
                hpbar.color += new Color(0, 0, 0, 0.01f);
                yield return null;
            }
        }
        else if (hpbar.color.a > alpha) {
            while (hpbar.color.a > alpha) {
                hpbar.color -= new Color(0, 0, 0, 0.01f);
                yield return null;
            }
        }
        else {
            yield return null;
        }
    }
    IEnumerator MoveUI(float alpha)
    {
        Image moveBar = HP_Bar.GetComponent<Image>();
        if (moveBar.color.a < alpha) {
            while (moveBar.color.a < alpha) {
                moveBar.color += new Color(0, 0, 0, 0.01f);
                yield return null;
            }
        }
        else if (moveBar.color.a > alpha) {
            while (moveBar.color.a > alpha) {
                moveBar.color -= new Color(0, 0, 0, 0.01f);
                yield return null;
            }
        }
        else {
            yield return null;
        }
    }
    IEnumerator vibrate()
    {

        for (int deg = 0; deg <= 1080; deg += 60) {
            transform.position += new Vector3(Mathf.Sin(deg * Mathf.Deg2Rad), 0, 0);
            deg++;
            yield return null;
        }
    }
    IEnumerator col_dam()
    {
        HPSurface.GetComponent<Image>().color = new Color(255f, 0, 0);
        while (HPSurface.GetComponent<Image>().color.g < 255) {
            HPSurface.GetComponent<Image>().color += new Color(0, 0.05f, 0.05f, 0);
            yield return null;
        }
    }

    IEnumerator hp_damp(float change)
    {
        Image hpBar = HP_Bar.GetComponent<Image>();

        if (hpBar.fillAmount < change) {
            while (hpBar.fillAmount < change) {
                hpBar.fillAmount += 0.01f;
                yield return null;
            }
        }
        else {
            while (hpBar.fillAmount > change) {
                hpBar.fillAmount -= 0.01f;
                yield return null;
            }
        }
    }
}