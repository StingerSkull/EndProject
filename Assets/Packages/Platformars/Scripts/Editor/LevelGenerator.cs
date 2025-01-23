using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace Platformars
{
    public class LevelGenerator : Editor
    {
		#region fields
		static int rhythmLength;
        static int minPopulation = 1000;
        static int maxPopulation = 1000;
        // double crossoverProbability = 0.5f;
        // double mutationProbability = 0.1f;
        static double desiredDifficulty;

        static int[,] level;
        static int[] rhythmInArray;
        static TileBase currTileBase;
        static GameObject levelObject;
        //static GameObject tilemapObject;
		#endregion

		#region methods
		/// <summary>
		/// Generate Level using Genetic Algorithm
		/// </summary>
		/// <param name="index">Level index</param>
		/// <param name="enemies">Enemy prefabs to be placed to generated levels</param>
		/// <param name="environments">Environment prefabs to be placed to generated levels</param>
		/// <param name="thresholdEasy">Sample easy level as a threshold for generated level difficulty</param>
		/// <param name="thresholdHard">Sample hard level as a threshold for generated level difficulty</param>
		/// <param name="avatar">Playable Character Game Object</param>
		/// <param name="difficulty">Level difficulty</param>
		public static void GenerateLevel(string identifiers,int index, GameObject[] enemies, TileBase[] environments, GameObject thresholdEasy, GameObject thresholdHard, GameObject avatar, float difficulty, string saveFolder)
        {
            //Extract level specification from thresholds
            int easyLength = thresholdEasy.GetComponentInChildren<Tilemap>().size.x;
            int hardLength = thresholdHard.GetComponentInChildren<Tilemap>().size.x;
            int easyHeight = thresholdEasy.GetComponentInChildren<Tilemap>().size.y;
            int hardHeight = thresholdHard.GetComponentInChildren<Tilemap>().size.y;

            //Count level height and length
            int levelLength = easyLength + Convert.ToInt32((float)Math.Abs(hardLength - easyLength) * difficulty * 5);
            int levelHeight = easyHeight + Convert.ToInt32((float)Math.Abs(hardHeight - easyHeight) * difficulty * 5);

            rhythmLength = levelLength;

            //Count level difficulty
            Debug.Log("difficulty scale : " + difficulty);
            desiredDifficulty = difficulty * rhythmLength; //Level difficulty is scaled based on rhythm length

            //Get avatar or playable character's height
            GameObject avatarInstance = Instantiate(avatar, new Vector3(0, 0, 0), Quaternion.identity);
            int avatarHeight = (int)avatarInstance.GetComponent<Collider2D>().bounds.size.y;

            //Debug mode
            //Debug.Log("avatar size : " + avatarInstance.GetComponent<Collider2D>().bounds.size);
            //Debug.Log("avatar height : " + avatarHeight);

            //Categorize enemies and obstacles, you can add more list for another type of obstacles
            List<GameObject> destructables = new List<GameObject>();
            List<GameObject> flyingEnemies = new List<GameObject>();
            List<GameObject> groundgEnemies = new List<GameObject>();
            List<GameObject> hazards = new List<GameObject>();

            CategorizeObstacles(destructables, flyingEnemies, groundgEnemies, hazards, enemies);

            //Assign tile base based on level difficulty
            if (environments.Length > 1)
            {
                int envIndex = (int)(difficulty / (1f / (float)environments.Length));
                currTileBase = environments[envIndex];
            }
            else currTileBase = environments[0];

            //Instantiate Genetic Algorithm
            var selection = new EliteSelection();
            var crossover = new UniformCrossover();
            var mutation = new UniformMutation();
            var fitness = new FitnessAgent(desiredDifficulty);
            var rhythm = new Rhythm(rhythmLength);

            var population = new Population(minPopulation, maxPopulation, rhythm);

            var mapGenerator = new MapGenerator();

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);
            ga.Termination = new FitnessStagnationTermination(100); // Terminate condition, 100 generation fitness stagnation

            level = new int[levelHeight, levelLength];

            ga.Start();
            Debug.Log("Best chromosome difficulty : " + fitness.CalculateDifficultyLevel(ga.BestChromosome));

            rhythmInArray = new int[levelLength];

            for (int i = 0; i < levelLength; i++)
            {
                rhythmInArray[i] = (int)ga.BestChromosome.GetGenes()[i].Value;
            }
            

            //Instantiate Map Object for environment
            int lastUsedRow = levelHeight / 2;

            GameObject grid = Instantiate(new GameObject("Map"), Vector3.zero, Quaternion.identity) as GameObject;
            grid.name = "Level" + "Map";
            grid.AddComponent<Grid>();
            GameObject tilemapObject = Instantiate(new GameObject("Tilemap"), Vector3.zero, Quaternion.identity) as GameObject;
            tilemapObject.AddComponent<Tilemap>();
            tilemapObject.AddComponent<TilemapRenderer>();
            tilemapObject.AddComponent<TilemapCollider2D>();
            tilemapObject.transform.SetParent(grid.transform);

            Tilemap tilemap = tilemapObject.GetComponent<Tilemap>();

            //Instantiate Obstacle Parent Object
            GameObject obstacleParent = Instantiate(new GameObject("Obstacles"), Vector3.zero, Quaternion.identity) as GameObject;

            //Generate actions in level matrix
            for (int i = 0; i < rhythmInArray.Length; i++)
            {
                if (rhythmInArray[i] == 0) lastUsedRow = GenerateRunAction(lastUsedRow, i, avatarHeight);
                if (rhythmInArray[i] == 1) lastUsedRow = GenerateJumpAction(lastUsedRow, i, levelHeight, avatarHeight);
                if (rhythmInArray[i] == 2) lastUsedRow = GenerateAttackAction(lastUsedRow, i,levelHeight, avatarHeight);
            }

            //Reverse level's column in place
            Debug.Log("length = "+levelHeight+", height = "+levelLength);
            ReverseColumnsInPlace(level);

            //Render Tile and Place Objects
            mapGenerator.DrawTile(level, currTileBase, tilemap);
            PlaceObstacles(level, difficulty, obstacleParent, destructables, flyingEnemies, groundgEnemies, hazards);

            //Save generated level prefabs to save folder
            levelObject = Instantiate(new GameObject(identifiers + "Level " + index), Vector3.zero, Quaternion.identity) as GameObject;
            grid.transform.SetParent(levelObject.transform);
            obstacleParent.transform.SetParent(levelObject.transform);
            PrefabUtility.SaveAsPrefabAsset(levelObject, saveFolder + "/" + levelObject.name + ".prefab");
            GameObject.DestroyImmediate(levelObject);
        }

        /// <summary>
        /// Generate run action in level matrix
        /// </summary>
        /// <param name="row">Level's current row position</param>
        /// <param name="column">Level's current column position</param>
        /// <param name="avatarHeight">Player avattar's height from collider's bound</param>
        /// <returns></returns>
        private static int GenerateRunAction(int row, int column, int avatarHeight)
        {
            //Set run action in level matrix
            // level[row, column] = 1;
            int rowMod = column > 0 && rhythmInArray[column - 1] == 1 ? 0 : UnityEngine.Random.Range(0, avatarHeight * 2);
            int nextRow = row + rowMod < level.GetLength(0) ? row + rowMod : row;
            level[nextRow, column] = 1;

            // int fillStart = row + 1 + random.Next(sprite.minVerticalGap, sprite.maxVerticalGap + 1);
            // int fillStart = row + UnityEngine.Random.Range(1, avatarHeight * 2); //temp assignment
            int fillStart = nextRow + 1;
            for (int i = fillStart; i < level.GetLength(0); i++) level[i, column] = 1;

            return nextRow;
        }

        /// <summary>
        /// Generate jump action in level matrix
        /// </summary>
        /// <param name="row">Level's current row position</param>
        /// <param name="column">Level's current column position</param>
        /// <param name="height">Level height</param>
        /// <param name="avatarHeight">Player avattar's height from collider's bound</param>
        /// <returns></returns>
        private static int GenerateJumpAction(int row, int column, int height, int avatarHeight)
        {
            //Count position for action placement
            int rowMod = UnityEngine.Random.Range(1, avatarHeight * 2);
            int nextRow = row - rowMod;
            if (nextRow < avatarHeight) nextRow = height - avatarHeight;
            if (nextRow > height - 2) nextRow = height - 2;

            //Set jump action in level matrix
            level[nextRow, column] = 2;

            //Fill Tilemaps with platform
            //int fillStart = nextRow + 1 + random.Next(sprite.minVerticalGap, sprite.maxVerticalGap + 1);
            int fillStart = nextRow + UnityEngine.Random.Range(1, avatarHeight * 2); //temp assignment
            for (int i = fillStart; i < height; i++) level[i, column] = 1;

            //if (sprite.isPlatform == true) return nextRow;
            // if (level[row, column] == 1) return nextRow;
            // else return row;
            return nextRow;
        }

        /// <summary>
        /// Generate attack action in level matrix
        /// </summary>
        /// <param name="row">Level's current row position</param>
        /// <param name="column">Level's current column position</param>
        /// <param name="height">Level height</param>
        /// <param name="avatarHeight">Player avattar's height from collider's bound</param>
        /// <returns></returns>
        private static int GenerateAttackAction(int row, int column,int height, int avatarHeight)
        {
            //Set attack action in level matrix
            // int nextRow = row - 1;
            int rowMod = 1; //temp assignment
            int nextRow = row - rowMod;
            if (nextRow < avatarHeight) nextRow = height - avatarHeight;
            if (nextRow > height - 2) nextRow = height - 2;
            level[nextRow, column] = 3;

            // //Fill Tilemaps with platform
            // //int fillStart = nextRow + 1 + random.Next(sprite.minVerticalGap, sprite.maxVerticalGap + 1);
            // int fillStart = nextRow + UnityEngine.Random.Range(1, avatarHeight + 1);//temp assignment
            int fillStart = nextRow + 1; //temp assignment
            for (int i = fillStart; i < height; i++) level[i, column] = 1;

            return row;
        }

        /// <summary>
        /// Categorize obstacles and add them to the list based on their obstacle type.
        /// You can modify this method like adding more parameters or even changing the behaviour to categorize obstacles for your projects
        /// If you want add more categories, make sure that you have added more obstacle types in Enemy.cs and Obstacle.cs
        /// </summary>
        /// <param name="destructables">List of harmless obstacle that can be destroyed(destructible objects)</param>
        /// <param name="groundEnemies">List of ground enemies</param>
        /// <param name="flyingEnemies">List of flying enemies</param>
        /// <param name="hazards">List of hazard object</param>
        /// <param name="enemies">List of enemies or obstacles get from input field, uncategorized</param>
        private static void CategorizeObstacles(List<GameObject> destructables, List<GameObject> groundEnemies, List<GameObject> flyingEnemies, List<GameObject> hazards, GameObject[] enemies)
        {
            foreach(GameObject obs in enemies)
            {
                if(obs.GetComponent<Obstacle>() != null)
                {
                    if (obs.GetComponent<Obstacle>().obstacleType == ObstacleType.Destroyable) destructables.Add(obs);
                    else hazards.Add(obs);
                }
                else if (obs.GetComponent<Enemy>() != null)
                {
                    if (obs.GetComponent<Enemy>().enemyType == EnemyType.EnemyFlying) flyingEnemies.Add(obs);
                    else groundEnemies.Add(obs);
                }
                else
                {
                    Debug.LogWarning("Uncategorized game object detected! GameObject name: " + obs.name + ".");
                    Debug.LogWarning("Uncategorized Game object " + obs.name + "will be categorized as Hazard!");
                    hazards.Add(obs);
                }
            }
        }

        /// <summary>
        /// Reverse matrix columns. Tilemap has different axis direction, so the matrix needs to be reversed before putting it into tilemap
        /// </summary>
        /// <param name="matrix">2d integer array</param>
        private static void ReverseColumnsInPlace(int[,] matrix)
        {
            for (int col = 0; col < matrix.GetLength(1); col++)
            {
                for (int row = 0; row < matrix.GetLength(0) / 2; row++)
                {
                    int temp = matrix[row,col];
                    matrix[row,col] = matrix[matrix.GetLength(0) - row - 1,col];
                    matrix[matrix.GetLength(0) - row - 1,col] = temp;
                }
            }
        }

        /// <summary>
        /// Placing obstacle and set parent obstacle.
        /// You can modify this method like adding more parameters or even changing the behaviour to place obstacles for your projects
        /// </summary>
        /// <param name="level">Level in matrix form</param>
        /// <param name="parent">Parent object for placed obstacle</param>
        /// <param name="destructables">List of destructable obstacle</param>
        /// <param name="groundEnemies">List of ground enemies</param>
        /// <param name="flyingEnemies">List of flying enemies</param>
        /// <param name="hazards">List of hazards object</param>
        private static void PlaceObstacles(int[,] level, float difficulty, GameObject parent, List<GameObject> destructables, List<GameObject> groundEnemies, List<GameObject> flyingEnemies, List<GameObject> hazards)
        {
            int hazardRange;
            int attackRange;

            GameObject instance;
            List<GameObject> attackAction = new List<GameObject>();
            attackAction.AddRange(groundEnemies);
            attackAction.AddRange(destructables);
            attackAction.AddRange(flyingEnemies);

            //Set obstacle Range based from difficulty
            hazardRange = (int)(difficulty / (1f / (float)(hazards.Count)));
            attackRange = (int)(difficulty / (1f / (float)(attackAction.Count)));


            for (int y = 0; y < level.GetLength(0); y++)
            {
                for (int x = 0; x < level.GetLength(1); x++)
                {
                    //Placing hazard
                    if (level[y, x] == 2 && hazards.Count > 0)
                    {
                        
                        if (hazards.Count > 1 && hazards.Count != 0)
                        {
                            instance = Instantiate(hazards[UnityEngine.Random.Range(0, hazardRange)], new Vector3Int(x, y, 0), Quaternion.identity);
                        }
                        else
                        {
                            instance = Instantiate(hazards[0], new Vector3Int(x, y, 0), Quaternion.identity);
                        }
                        instance.transform.SetParent(parent.transform);
                    }

                    //Placing enemies or destroytables
                    else if (level[y, x] == 3 && attackAction.Count > 0)
                    {
                        if (attackAction.Count > 1 && attackAction.Count != 0)
                        {
                            instance = Instantiate(attackAction[UnityEngine.Random.Range(0, attackRange)], new Vector3Int(x, y, 0), Quaternion.identity);
                        }
                        else
                        {
                            instance = Instantiate(attackAction[0], new Vector3Int(x, y, 0), Quaternion.identity);
                        }
                        instance.transform.SetParent(parent.transform);

                    }
                }
            }
            
        }
        #endregion
    }
}

