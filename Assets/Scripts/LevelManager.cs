using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	[SerializeField] WallContainer[] wallContainerPrefabs;
	[SerializeField] Player player;
	[SerializeField] float ySpeed;

	private float visibleHeight;
	private float visibleWidth;

	private float lastY;
	private WallContainer bottomWall;

	private List<WallContainer> visibleWalls = new List<WallContainer>();
	private Dictionary<int, List<WallContainer>> cachedWalls = new Dictionary<int, List<WallContainer>>();


	void Start() {
		visibleHeight = Camera.main.orthographicSize * 2f;
		visibleWidth = visibleHeight * Camera.main.aspect;

		for (int i = 0; i < wallContainerPrefabs.Length; i++) {
			wallContainerPrefabs [i].wallId = i;
		}

		lastY = Camera.main.transform.position.y - Camera.main.orthographicSize;
		while (needsNewWall ()) {
			addRandomWall ();
		}

		bottomWall = visibleWalls [0];
	}

	private void addRandomWall() {
		WallContainer container = getNextWallContainer ();

		float containerHeight = container.height;
		float yPos = lastY + (containerHeight / 2f);

		container.transform.position = new Vector3 (0f, yPos, 0f);

		lastY = lastY + containerHeight;
		container.topY = lastY;

		visibleWalls.Add(container);
	}

	private bool needsNewWall() {
		float topY = Camera.main.transform.position.y + Camera.main.orthographicSize + 1f; // agregar 1 para que haya margen
		return lastY < topY;
	}

	private WallContainer getNextWallContainer() {
		WallContainer container = null;

		int index = Random.Range (0, wallContainerPrefabs.Length - 1);
		List<WallContainer> cache = getCachedWalls (index);
		if (cache != null && cache.Count > 0) {
			container = cache [0];
			cache.Remove (container);
		} else {
			WallContainer prefab = wallContainerPrefabs [index];
			container = (WallContainer)GameObject.Instantiate (prefab);
			container.wallId = prefab.wallId;
			container.transform.SetParent (transform);
		}

		container.gameObject.SetActive (true);
		return container;
	}

	void Update() {
		if (needsNewWall ()) {
			addRandomWall ();
		}
		checkCacheBottomWall ();
	}

	private void checkCacheBottomWall() {
		float bottomY = Camera.main.transform.position.y - Camera.main.orthographicSize;

		if (bottomY > bottomWall.topY) {
			moveWallToCache (bottomWall);
			bottomWall = visibleWalls [0];
		}
	}

	private void moveWallToCache(WallContainer wall) {
		wall.gameObject.SetActive (false);
		visibleWalls.Remove (wall);
		List<WallContainer> equivalentWalls = getCachedWalls(wall.wallId);
		if (equivalentWalls == null) {
			equivalentWalls = new List<WallContainer> ();
			cachedWalls [wall.wallId] = equivalentWalls;
		}
		equivalentWalls.Add (wall);
	}

	private List<WallContainer> getCachedWalls(int wallId) {
		return cachedWalls.ContainsKey(wallId) ? cachedWalls [wallId] : null;
	}
}
