using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {

    public bool attacking = false;
    public bool defending = false;
    public bool takingdamage = false;
    private float time = 0.0f;
    Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Time =" + time);
        if (Input.GetKey("up") && time == 0) {
            attacking = true;
            anim.SetBool("attacking", true);
            time = 1.0f;
        }
        else if (time <=  0.0f)
        {
            anim.SetBool("attacking", false);
            attacking = false;
            defending = false;
            takingdamage = false;
            time = 0.0f;
        } else if (time > 0)
        {
            time -= 0.01f;
        }
    }
}
