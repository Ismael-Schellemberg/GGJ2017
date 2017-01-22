﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [SerializeField] WallContainer[] wallsFrequent;
    [SerializeField] WallContainer[] wallsRare;
    [SerializeField] Player player;
    [SerializeField] float xSpeed;
    [SerializeField] int minWallsBetweenRares = 5;

    private float visibleHeight;
    private float visibleWidth;
    private float cameraHorizontalSize;

    private float lastX;
    private WallContainer firstWall;

    private int wallsSinceLastRare = 0;


    private List<WallContainer> visibleWalls = new List<WallContainer>();

    private bool initialized = false;

    //    void Start() {
    //		init ();
    //    }

    void init() {
        if(!initialized) {
            initialized = true;

            visibleHeight = 2.0f * (-Camera.main.transform.position.z) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            //        visibleHeight = Camera.main.orthographicSize * 2f;
            visibleWidth = visibleHeight * Camera.main.aspect;
            cameraHorizontalSize = visibleWidth / 2f;

            lastX = -cameraHorizontalSize;
//			Debug.Log ("first lastX = " + lastX);

            for(int i = 0; i < wallsFrequent.Length; i++) {
                wallsFrequent[i].Init();
                wallsFrequent[i].wallId = i;
            }

            for(int i = 0; i < wallsRare.Length; i++) {
                wallsRare[i].Init();
                wallsRare[i].wallId = wallsFrequent.Length + i;
            }
        }
    }

    private void addRandomWall() {
        WallContainer container = getNextWallContainer();

        float containerWidth = container.width;
        float xPos = lastX + (containerWidth / 2f);


        container.transform.position = new Vector3(xPos, 0f, 0f);

        lastX = lastX + containerWidth;

//		Debug.Log ("Adding wall at " + xPos + ", new lastX = " + lastX);
//        container.maxX = lastX;

        visibleWalls.Add(container);
    }

    private bool needsNewWall() {
        float rightX = Camera.main.transform.position.x + cameraHorizontalSize + 1f; // agregar 1 para que haya margen
        return lastX < rightX;
    }

    private WallContainer getNextWallContainer() {
        WallContainer container = null;

        bool useRareWall = wallsSinceLastRare > minWallsBetweenRares ? Random.value < 0.2f : false;
        
        int maxIndex = useRareWall ? wallsRare.Length : wallsFrequent.Length;
        int index = Random.Range(0, maxIndex);
        int wallId = index;
        if(useRareWall) {
            wallId += wallsFrequent.Length;
            wallsSinceLastRare = 0;
        } else {
            wallsSinceLastRare++;
        }
				
        WallContainer prefab = useRareWall ? wallsRare[index] : wallsFrequent[index];
        container = (WallContainer)GameObject.Instantiate(prefab);
        container.wallId = prefab.wallId;
        container.Init();
        container.transform.SetParent(transform);

        container.gameObject.SetActive(true);
        return container;
    }

    public void Reset() {
        init();

        WallContainer[] toDelete = visibleWalls.ToArray();
        visibleWalls.Clear();
        for(int i = 0; i < toDelete.Length; i++) {
            Destroy(toDelete[i].gameObject);
        }
        firstWall = null;
        lastX = -cameraHorizontalSize;
        while(needsNewWall()) {
            addRandomWall();
        }
        firstWall = visibleWalls[0];
    }

    void Update() {
        if(needsNewWall()) {
            addRandomWall();
        }
        checkCacheFirstWall();
    }

    private void checkCacheFirstWall() {
        float leftX = Camera.main.transform.position.x - cameraHorizontalSize;

        if(firstWall != null && leftX > firstWall.maxX) {
            firstWall.gameObject.SetActive(false);
            visibleWalls.Remove(firstWall);
            Destroy(firstWall.gameObject);
            firstWall = null;

            if(visibleWalls.Count > 0)
                firstWall = visibleWalls[0];
        }
    }
}
