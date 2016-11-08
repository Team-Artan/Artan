using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Health_UI : MonoBehaviour {

    public GameObject targetTank;
    public float fullHP = 100;
    GameObject HPSurface;
    GameObject HP_Bar;
    GameObject Move_Bar;
    float HP;
    string cur_state;
    string temp_state;
	// Use this for initialization
	void Start () {
        HPSurface = GameObject.Find("HP_Surface");
        HP_Bar = GameObject.Find("HP_Bar");
        Move_Bar = GameObject.Find("Move_Bar");
        HP = fullHP;
        cur_state = "Stop";
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(targetTank.transform.position.x, targetTank.transform.position.y+0.3f, targetTank.transform.position.z);

        ChangeUI();
        if (Input.GetKeyDown(KeyCode.M))
        {
            if(cur_state == "Move")
            {
                cur_state = "Stop";
            }
            else
            {
                cur_state = "Move";
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetDamaged(20);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetHeal(20);
        }
    }

    public void GetDamaged(float damage)
    {
        StopAllCoroutines();
        StartCoroutine(vibrate());
        StartCoroutine(col_dam());
        
        if (HP_Bar.GetComponent<Image>().fillAmount > 0)
        {
            if (HP_Bar.GetComponent<Image>().fillAmount - damage / fullHP >= 0)
            {
                //HP_Bar.GetComponent<Image>().fillAmount -= damage / fullHP;
                StartCoroutine(hp_damp(HP_Bar.GetComponent<Image>().fillAmount - damage / fullHP));
                
            }
            else
            {
                //HP_Bar.GetComponent<Image>().fillAmount = 0;
                StartCoroutine(hp_damp(0));
            }
        }
    }
    public void GetHeal(float heal)
    {
        if (HP_Bar.GetComponent<Image>().fillAmount < 1)
        {
            if (HP_Bar.GetComponent<Image>().fillAmount + heal / fullHP <= 1)
            {
                StartCoroutine(hp_damp(HP_Bar.GetComponent<Image>().fillAmount + heal / fullHP));
            }
            else
            {
                StartCoroutine(hp_damp(1));
            }
        }
    }
    public void ChangeUI()
    {
        if(cur_state == "Move")
        {
            StartCoroutine(MoveUI(0.4f));
            StartCoroutine(HealthUI(0));
        }else if(cur_state == "Stop")
        {
            StartCoroutine(MoveUI(0));
            StartCoroutine(HealthUI(0.4f));
        }
    }
    IEnumerator HealthUI(float alpha)
    {
        Image hpbar = HP_Bar.GetComponent<Image>();
        if(hpbar.color.a < alpha)
        {
            while(hpbar.color.a < alpha)
            {
                hpbar.color += new Color(0, 0, 0, 0.01f);
                yield return null;
            }
        }
        else if(hpbar.color.a > alpha)
        {
            while(hpbar.color.a > alpha)
            {
                hpbar.color -= new Color(0, 0, 0, 0.01f);
                yield return null;
            }
        }else
        {
            yield return null;
        }
    }
    IEnumerator MoveUI(float alpha)
    {
        Image moveBar = Move_Bar.GetComponent<Image>();
        if(moveBar.color.a < alpha)
        {
            while(moveBar.color.a < alpha)
            {
                moveBar.color += new Color(0, 0, 0, 0.01f);
                yield return null;
            }
        }else if(moveBar.color.a > alpha)
        {
            while(moveBar.color.a > alpha)
            {
                moveBar.color -= new Color(0, 0, 0, 0.01f);
                yield return null;
            }
        }else
        {
            yield return null;
        }
    }
    IEnumerator vibrate()
    {
        
        for (int deg = 0; deg <= 1080; deg+=60)
        {
            transform.position += new Vector3(Mathf.Sin(deg * Mathf.Deg2Rad), 0, 0);
            deg++;
            yield return null;
        }
    }
    IEnumerator col_dam()
    {
        HPSurface.GetComponent<Image>().color = new Color(255f, 0, 0);
        while (HPSurface.GetComponent<Image>().color.g < 255)
        {
            HPSurface.GetComponent<Image>().color += new Color(0, 0.05f, 0.05f, 0);
            yield return null;
        }
    }

    IEnumerator hp_damp(float change)
    {
        Image hpBar = HP_Bar.GetComponent<Image>();

        if(hpBar.fillAmount < change)
        {
            while(hpBar.fillAmount < change)
            {
                hpBar.fillAmount += 0.01f;
                yield return null;
            }
        }else
        {
            while(hpBar.fillAmount > change)
            {
                hpBar.fillAmount -= 0.01f;
                yield return null;
            }
        }
    }

 

}
