
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    public Transform startingLoc;
    public Vector3 loadLoc;
    public Quaternion loadRot;
    public bool firstSceneloaded = false;
    public GameObject ThirdPersonRig;
    public int currentWheels = 0;
    public bool carQuestStarted = false;
    public bool carRepaired = false;
    public List<string> collectedWheelIDs = new List<string>();

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            if (!firstSceneloaded)
            {
                Debug.Log("Spawned Character at starting Location");
                SpawnCharacter(startingLoc.position, startingLoc.rotation);
                firstSceneloaded = true;
            }
            else
            {
                Debug.Log("Spawned Character at new Location");
                SpawnCharacter(loadLoc, loadRot);
            }
        }

        loadWheels();
    }

    public void SpawnCharacter(Vector3 loc, Quaternion rot)
    {
        Debug.Log("SPAWNED THIRD PERSON");
        Instantiate(ThirdPersonRig, loc, rot);
    }

    public void setSpawnPoint(Vector3 loc, Quaternion rot)
    {
        loadLoc = loc;
        loadRot = rot;
    }

    public void wheelCollect(string wheelID)
    {
        if (!collectedWheelIDs.Contains(wheelID))
        {
            collectedWheelIDs.Add(wheelID);
            currentWheels++;

            Debug.Log("Collect" + wheelID);
        }
    }
    [YarnCommand("checkWheels")]
    public void checkWheels()
    {
        InMemoryVariableStorage vData =
            FindAnyObjectByType<InMemoryVariableStorage>();

        vData.SetValue("$wheelsCollected", (float)currentWheels);
        vData.SetValue("$carQuestStarted", carQuestStarted);


    }

    [YarnCommand("startCarQuest")]
    public void startCarQuest()
    {
        carQuestStarted = true;
    }

    [YarnCommand("repairCar")]
    public void repairCar()
    {
        carRepaired = true;
    }
    public void loadWheels()
    {
        Wheel_Interact[] allWheelsInScene =
            FindObjectsByType<Wheel_Interact>(FindObjectsSortMode.None);

        foreach (Wheel_Interact wheel in allWheelsInScene)
        {
            if (collectedWheelIDs.Contains(wheel.wheelID))
            {
                Destroy(wheel.gameObject);
            }
        }
    }

    public void setPlayerMovement(bool b)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        ThirdPersonController tpc = player.GetComponent<ThirdPersonController>();

        if (b)
        {
            tpc.enabled = false;
        }
        else
        {
            tpc.enabled = true;
        }
    }

    public void moveForDialog(Transform movePos)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = movePos.position;
    }
}
