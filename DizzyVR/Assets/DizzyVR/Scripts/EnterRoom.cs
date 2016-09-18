using UnityEngine;
using System.Collections;

public class EnterRoom : MonoBehaviour {

	public SphereOfBlindness sphere;
	public int room_id;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other){
		string log = string.Format("fired at room id {0}", room_id);
		Debug.Log(log);
		if (room_id == sphere.correct_room){
			gameObject.SetActive(false);
		}		
	}

	void OnTriggerExit(Collider other){
		gameObject.SetActive(true);
	}

	// Update is called once per frame
	void Update () {
	
	}
}