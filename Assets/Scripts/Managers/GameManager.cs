using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class GameManager : MonoBehaviour
{
    public int activeIndex;
    public int enemyActiveIndex;
    public List<ArenaController> arenaList = new List<ArenaController>();
    //public List<ArenaController> enemyArenaList = new List<ArenaController>();
    public static GameManager Instance { get; private set; }

    private IAdaptivePerformance ap = null;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {
        ap = Holder.Instance;

        Application.targetFrameRate = 120;
        QualitySettings.lodBias = 1.0f;
        QualitySettings.vSyncCount = 0;
        Screen.SetResolution(Screen.width, Screen.height, true, 60);

        if (ap != null)
        {
            if (!ap.Active)
                return;
            else
                OnBenchmark();
        }

        //arenaList = FindObjectsOfType<ArenaController>().OrderBy(x => x.name).ToList();
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        activeIndex = PlayerMovement.Instance.currentLevelIndex;
        //enemyActiveIndex = (RotateObject.Instance.currentLevelIndex) - 2;
        if(InputHandler.Instance._state != InputHandler.state.CHOOSE)
        CheckList();
    }

    public void CheckList()
    {

        for (int i = 0; i < arenaList.Count; i++)
        {
            if (arenaList[i] != arenaList[activeIndex])
                arenaList[i].active = false;
            else
                arenaList[activeIndex].active = true;
        }
        /*
        for (int i = 0; i < enemyArenaList.Count; i++)
        {
            if (enemyArenaList[i] != enemyArenaList[enemyActiveIndex + 3])
                enemyArenaList[i].active = false;
            else
                enemyArenaList[activeIndex].active = true;
        }
        */
        //var GO = arenaList.Find(x => x.GetComponentInChildren<SpriteRenderer>() != null);
        //GO.enabled = false;
    }

    public void OnBenchmark()
    {
        var ctrl = ap.DevicePerformanceControl;
        ctrl.CpuPerformanceBoost = true;
        ctrl.GpuPerformanceBoost = true;
        // Set higher CPU and GPU level when benchmarking a level
        ctrl.CpuLevel = ctrl.MaxCpuPerformanceLevel;
        ctrl.GpuLevel = ctrl.MaxGpuPerformanceLevel;
    }
}
