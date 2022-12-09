using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int activeIndex;
    public int enemyActiveIndex;
    public List<ArenaController> arenaList = new List<ArenaController>();
    //public List<ArenaController> enemyArenaList = new List<ArenaController>();
    public static GameManager Instance { get; private set; }
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
        //arenaList = FindObjectsOfType<ArenaController>().OrderBy(x => x.name).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        activeIndex = RotateObject.Instance.currentLevelIndex;
        //enemyActiveIndex = (RotateObject.Instance.currentLevelIndex) - 2;
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
}
