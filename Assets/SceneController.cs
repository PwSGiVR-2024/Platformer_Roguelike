using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private int shopSceneIndex;
    [SerializeField] private int lastIndexOfSceneSpawned = 0;
    [SerializeField] private Vector3 moveAmount = new Vector3(37, 0, 0);
    [SerializeField] private List<int> floorSizes;
    [SerializeField] private List<int> indexForBossScene;
    public List<int> loadedSceneIndexes = new List<int>();
    [SerializeField] private MapTranistion mapInstance;
    [SerializeField] private Dictionary<int, List<int>> floorSceneIndexes = new Dictionary<int, List<int>>();
    [SerializeField] private GameObject player;
    [SerializeField] private int indexOfDeathScene;
    [SerializeField] private int indexOfSecondFloorsScene;

    //public Action changeEnemyPos;

    private const int minFloorSize = 2;
    private const int maxFloorSize = 5;
    public int currentFloor = 0;
    public int loadedScenes;
    private int indexOfSceneToSpawn;
    private Vector3 VectorOfYPostionFirstScene;
    private List<int> shopInsertedFloors = new List<int>();
    [SerializeField] private bool isShopLoaded = false;
    [SerializeField] private bool hasShopBeenLoaded = false;
    float lastPosition = 0f;
    void Start()
    {
        loadedSceneIndexes.Add(0); //pierwsza za�adowana scena to 0
        StartCoroutine(InitializeFloorsCoroutine()); //zacznij losowanie indexow do konkretnych poziomow
    }

    private IEnumerator InitializeFloorsCoroutine()
    {
        for (int floor = 0; floor < floorSizes.Count; floor++)
        {
            Debug.Log("Zacz�to losowa� pi�tro: " + floor);
            yield return StartCoroutine(InitializeFloorScenesCoroutine(floor));
        }
    }

    private IEnumerator InitializeFloorScenesCoroutine(int floor)
    {
        int floorSize = UnityEngine.Random.Range(minFloorSize, maxFloorSize);
        floorSizes[floor] = floorSize;
        var sceneIndexes = new HashSet<int>();

        for (int i = 0; i < floorSize; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = UnityEngine.Random.Range(1, 4);
                print("Losowany indeks: " + randomIndex);
            }
            while (sceneIndexes.Contains(randomIndex));

            if (!floorSceneIndexes.ContainsKey(floor))
            {
                floorSceneIndexes[floor] = new List<int>();
            }

            floorSceneIndexes[floor].Add(randomIndex);
            sceneIndexes.Add(randomIndex);
        }

        // Dodanie sklepu na losowej pozycji, ale nie pierwszej ani ostatniej
        if (!shopInsertedFloors.Contains(floor))
        {
            int shopPosition = UnityEngine.Random.Range(1, floorSize - 1);
            floorSceneIndexes[floor].Insert(shopPosition, shopSceneIndex);
            shopInsertedFloors.Add(floor);
        }

        floorSceneIndexes[floor].Add(indexForBossScene[floor]);

        yield return null; // Mo�esz doda� op�nienie lub zwrot warto�ci, je�li potrzebujesz
    }


public void LoadScene()
    {
        indexOfSceneToSpawn = floorSceneIndexes[currentFloor].First();
        if (IsSceneAlreadyLoaded(indexOfSceneToSpawn))
        {
            return;
        }
        if (indexOfSceneToSpawn == shopSceneIndex)
        {
            StartCoroutine(LoadSceneWithDelayCoroutine(5f));
        }
        StartCoroutine(LoadSceneCoroutine(LoadSceneMode.Additive));
    }

    public void onPlayersDeath()
    {
        SceneManager.LoadScene(indexOfDeathScene);
    }
    private IEnumerator LoadSceneCoroutine(LoadSceneMode mode)
    {
        floorSceneIndexes[currentFloor].Remove(indexOfSceneToSpawn);
        loadedScenes++;
        loadedSceneIndexes.Add(indexOfSceneToSpawn);
        yield return SceneManager.LoadSceneAsync(indexOfSceneToSpawn, mode);
        Scene scene = SceneManager.GetSceneByBuildIndex(indexOfSceneToSpawn);
        yield return StartCoroutine(MoveSceneCoroutine(scene)); // Dodaj wstrzymanie a� do zako�czenia przesuni�cia sceny
        SceneManager.SetActiveScene(scene);
        //changeEnemyPos?.Invoke();
    }

    public IEnumerator UnLoadScene()
    {
        if (loadedSceneIndexes.Count > 0)
        {
            int sceneIndexToUnload = loadedSceneIndexes.First();
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneIndexToUnload);
            yield return asyncUnload;

            loadedSceneIndexes.Remove(sceneIndexToUnload);
        }
        else
        {
            Debug.LogWarning("No scenes to unload.");
        }
    }

    private bool IsSceneAlreadyLoaded(int sceneIndex)
    {
        foreach (var scene in SceneManager.GetAllScenes())
        {
            if (scene.buildIndex == sceneIndex)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator MoveSceneCoroutine(Scene loadScene)
    {

        GameObject[] sceneObjects = loadScene.GetRootGameObjects();
        if (sceneObjects.Length > 0)
        {

            GameObject sceneObject = sceneObjects[0];
            Vector3 originalPosition = sceneObject.transform.position;
            if (isShopLoaded)
            {
                hasShopBeenLoaded = true;
                lastPosition = (moveAmount.x * (loadedScenes - 1)) + 25.5f;
                originalPosition.x = lastPosition;
            }
            else if (!isShopLoaded && !hasShopBeenLoaded)
            {
                originalPosition.x += moveAmount.x * loadedScenes;
            }
            else if (!isShopLoaded && hasShopBeenLoaded)
            {
                lastPosition += moveAmount.x;
                originalPosition.x = lastPosition;
            }
            VectorOfYPostionFirstScene = new Vector3(0, (int)(SceneManager.GetActiveScene().GetRootGameObjects()[0].transform.position.y), 0);
            originalPosition.y = VectorOfYPostionFirstScene.y;
            sceneObject.transform.position = originalPosition;

            if (loadScene.buildIndex == shopSceneIndex)
            {
                isShopLoaded = true;
            }
            else
            {
                isShopLoaded = false;
            }
            yield return null;
        }
    }

    private IEnumerator LoadSceneWithDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadScene();
    }

    public IEnumerator AfterBossDeathAsync()
    {
        foreach (int sceneIndex in loadedSceneIndexes)
        {
            yield return SceneManager.UnloadSceneAsync(sceneIndex);
        }
        loadedSceneIndexes.Clear();
        currentFloor++;
        hasShopBeenLoaded = false;
        yield return StartCoroutine(LoadFirstSceneOfNextFloor());
    }
    private IEnumerator LoadFirstSceneOfNextFloor()
    {
        if (floorSceneIndexes.ContainsKey(currentFloor) && floorSceneIndexes[currentFloor].Count > 0)
        {
            indexOfSceneToSpawn = floorSceneIndexes[currentFloor][0];
            floorSceneIndexes[currentFloor].RemoveAt(0);
            yield return StartCoroutine(LoadSceneCoroutine(LoadSceneMode.Single));
        }

        yield return null;
    }
    public int GetLastIndexOfSceneSpawned()
    {
        return lastIndexOfSceneSpawned;
    }
}
