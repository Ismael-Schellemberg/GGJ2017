using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [SerializeField] WallContainer[] wallContainerPrefabs;
    [SerializeField] Player player;
    [SerializeField] float xSpeed;

    private float visibleHeight;
    private float visibleWidth;

    private float lastX;
    private WallContainer bottomWall;

    private List<WallContainer> visibleWalls = new List<WallContainer>();
    private Dictionary<int, List<WallContainer>> cachedWalls = new Dictionary<int, List<WallContainer>>();


    void Start() {
        visibleHeight = Camera.main.orthographicSize * 2f;
        visibleWidth = visibleHeight * Camera.main.aspect;

        for(int i = 0; i < wallContainerPrefabs.Length; i++) {
            wallContainerPrefabs[i].wallId = i;
        }

        lastX = Camera.main.transform.position.x - Camera.main.orthographicSize;
        while(needsNewWall()) {
            addRandomWall();
        }

        bottomWall = visibleWalls[0];
    }

    private void addRandomWall() {
        WallContainer container = getNextWallContainer();

        float containerWidth = container.widht;
        float xPos = lastX + (containerWidth / 2f);

        container.transform.position = new Vector3(xPos, 0f, 0f);

        lastX = lastX + containerWidth;

        container.minX = lastX;

        visibleWalls.Add(container);
    }

    private bool needsNewWall() {
        float topX = Camera.main.transform.position.x + Camera.main.orthographicSize + 1f; // agregar 1 para que haya margen
        return lastX < topX;
    }

    private WallContainer getNextWallContainer() {
        WallContainer container = null;

        int index = Random.Range(0, wallContainerPrefabs.Length);
        List<WallContainer> cache = getCachedWalls(index);
        if(cache != null && cache.Count > 0) {
            container = cache[0];
            cache.Remove(container);
        } else {
            WallContainer prefab = wallContainerPrefabs[index];
            container = (WallContainer)GameObject.Instantiate(prefab);
            container.wallId = prefab.wallId;
            container.transform.SetParent(transform);
        }

        container.gameObject.SetActive(true);
        return container;
    }

    void Update() {
        if(needsNewWall()) {
            addRandomWall();
        }
        checkCacheBottomWall();
    }

    private void checkCacheBottomWall() {
        float leftX = Camera.main.transform.position.x - Camera.main.orthographicSize;

        if(leftX > bottomWall.minX) {
            moveWallToCache(bottomWall);
            bottomWall = visibleWalls[0];
        }
    }

    private void moveWallToCache(WallContainer wall) {
        wall.gameObject.SetActive(false);
        visibleWalls.Remove(wall);
        List<WallContainer> equivalentWalls = getCachedWalls(wall.wallId);
        if(equivalentWalls == null) {
            equivalentWalls = new List<WallContainer>();
            cachedWalls[wall.wallId] = equivalentWalls;
        }
        equivalentWalls.Add(wall);
    }

    private List<WallContainer> getCachedWalls(int wallId) {
        return cachedWalls.ContainsKey(wallId) ? cachedWalls[wallId] : null;
    }
}
