using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class TileManager : MonoBehaviour
{
    private List<TileData> _tiles;

    [SerializeField] private GameObject tileTest;

    [SerializeField] private int gridSize = 100;
    [SerializeField] private float tileSize = 1;
    [SerializeField] private float minRadius = 1;
    [SerializeField] private float maxRadius = 1;

    private void Start()
    {
        _tiles = new List<TileData>();
        
        /*var halfSize = (gridSize / 2) * tileSize;
        Vector3 tilePosition = new Vector3(-halfSize, 0, -halfSize);
        
        for (int i = 0; i < gridSize; i++)
        {
            tilePosition.z = -halfSize;
            
            for (int j = 0; j < gridSize; j++)
            {
                //Debug.Log(tilePosition + " ~ " + tilePosition.magnitude);
                if (tilePosition.magnitude < maxRadius && tilePosition.magnitude > minRadius)
                {
                    TileData newTile = new TileData();
                    newTile.Position = tilePosition;
                    newTile.Object = Instantiate(tileTest, tilePosition, quaternion.identity);    
                }

                tilePosition.z += tileSize;
            }

            tilePosition.x += tileSize;
        }*/
    }
}
