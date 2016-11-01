﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Health_UI : MonoBehaviour {

    public GameObject targetTank;
    public float fullHP = 100;
    GameObject HPSurface;
    GameObject HP_Bar;
    float HP;
	// Use this for initialization
	void Start () {
        HPSurface = GameObject.Find("HP_Surface");
        HP_Bar = GameObject.Find("HP_Bar");
        HP = fullHP;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(targetTank.transform.position.x, targetTank.transform.position.y+4f, targetTank.transform.position.z);
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
