﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    private Vector3 prePosition = Vector3.zero;
    private Rigidbody rigid;
    private int bounceCount = 0;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (rigid.velocity!=Vector3.zero&&prePosition!=null) {
            Vector3 relativePos = transform.position - prePosition;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = rotation * Quaternion.Euler(90,0,0);
        }
        prePosition = transform.position;
	}
}
