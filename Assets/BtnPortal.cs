using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class BtnPortal : MonoBehaviour
{
    public static BtnPortal Instance { get; private set; }

    public List<Button> BtnList = new List<Button>();
    public GameObject btnPrefabs;
    public ArenaController nextArena;
    int index;
    private void Awake()
    {
        
        //nextArena = null;
    }
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        GetNextArena();
    }

    private void OnDisable()
    {
        nextArena = null;
        BtnList = null;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        BtnList = GetComponentsInChildren<Button>().ToList();

        for (int i = 0; i < nextArena.portalList.Count; i++)
        {
            if (BtnList.Count < nextArena.portalList.Count)
            {
                var button = Instantiate(btnPrefabs, transform);
                button.GetComponentInChildren<Text>().text = (i + 1).ToString();
                button.name = nextArena.portalList[i].name;
                button.GetComponent<Button>().onClick.AddListener(nextArena.portalList[i].SelectThis);
                BtnList.Add(button.GetComponent<Button>());
            }
            
        }
    }

    public void GetNextArena()
    {
        index = ArenaManager.Instance.currentArenaIndex;
        nextArena = ArenaManager.Instance.controllerList[index += 1];
    }
}
