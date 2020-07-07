using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tilePrefabs;
    public float TileWidth
    {
        get
        {
            //All tiles are the same size
            return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Renders the tiles to create level
    /// </summary>
    private void CreateLevel()
    {
        string[] mapData = ReadLevelText();
        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;

        //Start position from the camera 
        Vector3 worldStartPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapY; y++) //Y
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX-1; x++) //X
            {
                PlaceTile(x, y, newTiles[x].ToString(), worldStartPos);
            }
        }
    }

    /// <summary>
    /// Places the tiles
    /// </summary>
    private void PlaceTile(int x, int y, string tileType, Vector3 worldStartPos)
    {
        int tileIndex = int.Parse(tileType);
        //Create a tile
        GameObject newTile = Instantiate(tilePrefabs[tileIndex]);
        newTile.transform.position = new Vector3(worldStartPos.x + TileWidth * x, worldStartPos.y - TileWidth * y);
    }

    /// <summary>
    /// Reads the level txt
    /// </summary>
    /// <returns>Array of tile file</returns>
    private string[] ReadLevelText()
    {
        TextAsset txtData = Resources.Load("Leve1") as TextAsset;
        return txtData.text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
    }



}
