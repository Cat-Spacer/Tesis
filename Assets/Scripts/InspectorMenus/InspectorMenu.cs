using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class InspectorMenu : EditorWindow
{
    private GameObjectData gameObjectData;
    List<GameObject> myGameObjects = new List<GameObject>();
    
    private GameObject pressurePlate;
    private GameObject fallingTrap;
    private GameObject turret;
    private GameObject fallingFloor;
    private GameObject floorSwitch;
    private GameObject flower;
    private GameObject jumpPad;
    private GameObject laser;
    private GameObject sawTrap;
    private GameObject smashTrap;
    private GameObject spike;
    private GameObject tpOneSided;
    private GameObject tpTwoSided;
    private GameObject catLever;
    private GameObject hamsterLever;

    private Transform parent;
    private Vector2 scrollPosition;
    [MenuItem("Window/CreateObstacle")]
    public static void ShowWindow()
    {
        GetWindow<InspectorMenu>("CreateObstacle");
    }
    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(400));
        for (int i = 0; i < gameObjectData.myGameObjects.Count; i++)
        {
            gameObjectData.myGameObjects[i] = DrawObjectField(gameObjectData.myGameObjects[i], $"Object {i + 1}", 200, 20);
        }
        
        if (GUILayout.Button("Find Parent"))
        {
            parent = GameObject.FindGameObjectWithTag("Levels").transform;
        }

        // pressurePlate = DrawObjectField("Pressure Plate", pressurePlate, 200);
        // fallingTrap = DrawObjectField("Falling Trap", fallingTrap, 200);
        // turret = DrawObjectField("Turret", turret, 200);
        // electricButton = DrawObjectField("Electric Button", electricButton, 200);
        // fallingFloor = DrawObjectField("Falling Floor", fallingFloor, 200);
        // floorSwitch = DrawObjectField("Floor Switch", floorSwitch, 200);
        // flower = DrawObjectField("Flower", flower, 200);
        // jumpPad = DrawObjectField("Jump Pad", jumpPad, 200);
        // laser = DrawObjectField("Laser", laser, 200);
        // sawTrap = DrawObjectField("Saw Trap", sawTrap, 200);
        // smashTrap = DrawObjectField("Smash Trap", smashTrap, 200);
        // spike = DrawObjectField("Spike", spike, 200);
        // tpOneSided = DrawObjectField("Tp One Sided", tpOneSided, 200);
        // tpTwoSided = DrawObjectField("Tp Two Sided", tpTwoSided, 200);

        EditorGUILayout.Space(2);
        parent = (Transform)EditorGUILayout.ObjectField("Parent", parent, typeof(Transform), true);

        EditorGUILayout.EndScrollView();
    }

    void CreateObstacle(GameObject prefab)
    {
        if (GUILayout.Button("Create", GUILayout.Width(100)))
        {
            var newObstacle = Instantiate(prefab);
            newObstacle.transform.position = new Vector3(SceneView.lastActiveSceneView.camera.transform.position.x, SceneView.lastActiveSceneView.camera.transform.position.y, 0);
            newObstacle.transform.parent = parent;
            Selection.activeObject = newObstacle;
        }
    }
    private GameObject DrawObjectField(GameObject obj, string label, float width, float height)
    {
        EditorGUILayout.BeginHorizontal();
        if(obj == null) EditorGUILayout.LabelField(label, GUILayout.Width(80));
        else EditorGUILayout.LabelField(obj.name, GUILayout.Width(80));
        obj = (GameObject)EditorGUILayout.ObjectField(obj, typeof(GameObject), true, GUILayout.Width(width), GUILayout.Height(height));
        if (GUILayout.Button("Create", GUILayout.Width(width)))
        {
            GameObject newObstacle = (GameObject)PrefabUtility.InstantiatePrefab(obj);
            newObstacle.transform.position = new Vector3(SceneView.lastActiveSceneView.camera.transform.position.x, SceneView.lastActiveSceneView.camera.transform.position.y, 0);
            newObstacle.transform.parent = parent;
            Selection.activeObject = newObstacle;
        }
        EditorGUILayout.EndHorizontal();

        return obj;
    }
    GameObject DrawObjectField(string label, GameObject obj, float width)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(label, EditorStyles.boldLabel);
        obj = (GameObject)EditorGUILayout.ObjectField(obj, typeof(GameObject), true, GUILayout.Width(width));
        if (GUILayout.Button("Create", GUILayout.Width(width)))
        {
            var newObstacle = Instantiate(obj);
            newObstacle.transform.position = new Vector3(SceneView.lastActiveSceneView.camera.transform.position.x, SceneView.lastActiveSceneView.camera.transform.position.y, 0);
            newObstacle.transform.parent = parent;
            Selection.activeObject = newObstacle;
        }
        EditorGUILayout.EndHorizontal();
        return obj;
    }
    private void OnEnable()
    {
        // Cargar el ScriptableObject desde Assets (asegúrate de tener uno creado)
        gameObjectData = AssetDatabase.LoadAssetAtPath<GameObjectData>("Assets/Scripts/InspectorMenus/Obstacles.asset");

        // Si no existe, crearlo y guardarlo
        if (gameObjectData == null)
        {
            gameObjectData = CreateInstance<GameObjectData>();
            AssetDatabase.CreateAsset(gameObjectData, "Assets/Scripts/InspectorMenus/Obstacles.asset");
            AssetDatabase.SaveAssets();
        }
    }

}
#endif