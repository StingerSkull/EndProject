using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Platformars
{
    public class ObstacleMakerWindow : EditorWindow
    {
		#region consts
		const string menu = "Tools/Platformars/Obstacle Maker Window";
		const string assetFolder = "Assets/Platformars";
		const string bannerIconAddress = assetFolder + "Textures/bannericon.png";
		const string desc = "Use this feature to define the enemy or obstacle type with game objects prefabs from your projects so the level generator can place the object based on their type";
		#endregion

		#region fields
		private Vector2 _scrollPos;
		#endregion

		#region properties
		public Texture bannerIcon;
		public GameObject[] enemies;
		public int index;
		public string[] options = { "Destroyable Obstacle", "Enemy (Ground)", "Enemy (Flying)", "Hazard" };
		#endregion

		#region methods
		[MenuItem(menu, false, 15)]
		public static void ShowWindow()
		{
			GetWindow<ObstacleMakerWindow>("Obstacle Maker");
		}

		//Tab layout
		private void OnGUI()
		{
			GUILayout.Box(bannerIcon, GUILayout.Height(Screen.width), GUILayout.Width(Screen.width));
			EditorGUILayout.HelpBox(desc, MessageType.Info);
			
			EditorGUILayout.BeginVertical();
			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

			//target refering to selected gameobject
			ScriptableObject target = this;
			SerializedObject go = new SerializedObject(target);

			SerializedProperty enemyProperty = go.FindProperty("enemies");
			EditorGUILayout.PropertyField(enemyProperty, true); // True means show children

			go.ApplyModifiedProperties(); // Remember to apply modified properties


			index = EditorGUILayout.Popup(
					"Obstacle Type",
					index,
					options);

			if (GUILayout.Button("Define Obstacle Type"))
			{
				if (enemies != null)
					DefineEnemy(enemies);
				else
					Debug.Log("GameObjects for obstacles cannot be Null!");
			}
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

		/// <summary>
		/// Define enemy type by adding new component to the GameObject
		/// </summary>
		/// <param name="enemies"></param>
		public void DefineEnemy(GameObject[] enemies)
		{
			int curr = 0;
			foreach (GameObject enemy in enemies)
			{
				if (enemy == null)
					Debug.Log("No GameObject at index = " + curr +"!");
				else
				{
					if(enemy.GetComponent<Collider2D>() == null)
					{
						Debug.LogWarning("Game object '" + enemy.name + "' has no 2D Collider in it. Consider checking the game object. Object type will still be added to the object.");
					}
					switch (index)
					{
						case 0:
							if (enemy.GetComponent<Obstacle>() == null)
								enemy.AddComponent<Obstacle>();
							if (enemy.GetComponent<Enemy>() != null)
								Object.DestroyImmediate(enemy.GetComponent<Enemy>() as Object, true);
							enemy.GetComponent<Obstacle>().obstacleType = ObstacleType.Destroyable;
							break;
						case 1:
							if (enemy.GetComponent<Enemy>() == null)
								enemy.AddComponent<Enemy>();
							if (enemy.GetComponent<Obstacle>() != null)
								Object.DestroyImmediate(enemy.GetComponent<Obstacle>() as Object, true);
							enemy.GetComponent<Enemy>().enemyType = EnemyType.EnemyGround;
							break;
						case 2:
							if (enemy.GetComponent<Enemy>() == null)
								enemy.AddComponent<Enemy>();
							if (enemy.GetComponent<Obstacle>() != null)
								Object.DestroyImmediate(enemy.GetComponent<Obstacle>() as Object, true);
							enemy.GetComponent<Enemy>().enemyType = EnemyType.EnemyFlying;
							break;
						case 3:
							if (enemy.GetComponent<Obstacle>() == null)
								enemy.AddComponent<Obstacle>();
							if (enemy.GetComponent<Enemy>() != null)
								Object.DestroyImmediate(enemy.GetComponent<Enemy>() as Object, true);
							enemy.GetComponent<Obstacle>().obstacleType = ObstacleType.Hazard;
							break;
					}
					Debug.Log(curr + ". GameObject categorized as '" + options[index] + "'!");
				}
				curr++;
			}
			//Debug.Log("All " + enemies.Length + " GameObjects categorized as" + options[index] + "!");
		}
		#endregion
	}
}

