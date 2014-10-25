using System;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class DaimondAnimBehavior : MonoBehaviour {

	public float BPM;

	// Use this for initialization
	void Start () {
		Camera cam = Camera.main;

		Vector3 bl = Camera.main.ViewportToWorldPoint(Vector3.zero);
		Vector3 ur = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

		float camWidth = ur.x - bl.x;

		float halfCamWidth = camWidth / 2f;
		float halfCamHeight = cam.orthographicSize;

		this.transform.position = new Vector3 (-(halfCamWidth * 0.6f), -(halfCamHeight * 0.75f), this.transform.position.z);

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPosition = new Vector3(this.transform.position.x,
                                                   -0.5f*(float)Math.Cos((BPM/60f)*Math.PI*Time.time), 
                                                   this.transform.position.z);

		this.transform.position = newPosition;
	}
}
