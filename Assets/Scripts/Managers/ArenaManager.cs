using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager Instance { get; private set; }

    public List<ArenaController> controllerList = new List<ArenaController>();
    public int currentArenaIndex;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentArenaIndex = PlayerMovement.Instance.currentLevelIndex;
    }
}
