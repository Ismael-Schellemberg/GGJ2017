using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	[SerializeField] GameObject wallPrefab;
	[SerializeField] Camera camera;
	[SerializeField] Player player;
	[SerializeField] float speed;

	private List<GameObject> leftWalls = new List<GameObject>();
	private List<GameObject> rightWalls = new List<GameObject>();



	void Start() {
		
	}

	void Update() {
//		float deltaY = speed * Time.deltaTime;
//		Vector3 movement = new Vector3 (0f, deltaY, 0f);
//		player.transform.Translate (movement);
//		camera.transform.Translate (movement);
	}
}
