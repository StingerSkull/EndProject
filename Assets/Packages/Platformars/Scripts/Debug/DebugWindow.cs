using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Platformars;


public class DebugWindow : EditorWindow
{
    Tilemap tilemap;
    TileBase environment;
    int[,] sampleArray = { 
        { 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0 }, 
        { 0, 0, 0, 0, 0, 0 }, 
        { 1, 1, 1, 1, 1, 1 }, 
        { 1, 0, 0, 1, 0, 1 } };


    [MenuItem("Tools/Debug/Debug Window")]
    public static void ShowWindow()
    {
        GetWindow<DebugWindow>("Debug Window");
    }

    private void OnGUI()
    {
        environment = (TileBase)EditorGUILayout.ObjectField(new GUIContent("Environment Tiles", "Tilebase"), environment, typeof(TileBase), true);
        if (GUILayout.Button("Create sample tilemap"))
        {
            CreateTilemap();
        }
    }

    public void CreateTilemap()
    {
        GameObject grid = Instantiate(new GameObject("Map"), Vector3.zero, Quaternion.identity) as GameObject;
        grid.AddComponent<Grid>();

        GameObject tilemapObject = Instantiate(new GameObject("Tilemap"), Vector3.zero, Quaternion.identity) as GameObject;
        tilemapObject.AddComponent<Tilemap>();
        tilemapObject.AddComponent<TilemapRenderer>();
        tilemapObject.AddComponent<TilemapCollider2D>();
        tilemapObject.transform.SetParent(grid.transform);
        tilemap = tilemapObject.GetComponent<Tilemap>();

        DrawTile(sampleArray, environment, tilemap);
    }

    public void DrawTile(int[,] map, TileBase tileBase, Tilemap tilemap)
    {
        
        if (map != null)
        {

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y] == 1)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), tileBase);

                    }
                    //Debug.Log(map[x, y] + " ");
                }
            }
        }


    }

}
