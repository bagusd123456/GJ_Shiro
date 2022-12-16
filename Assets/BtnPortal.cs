using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BtnPortal : MonoBehaviour
{

    public List<Button> BtnList = new List<Button>();
    public GameObject btnPrefabs;
    public ArenaController nextArena;
    private void Awake()
    {
        nextArena = ArenaManager.Instance.controllerList[ArenaManager.Instance.currentArenaIndex++];

    }
    private void Start()
    {
        
    }

    private void Update()
    {
  
        BtnList = GetComponentsInChildren<Button>().ToList();

        if(nextArena == null)
        {
            return;
        }
        for (int i = 0; i < nextArena.portalList.Count; i++)
        {
            var button = Instantiate(btnPrefabs, transform);

            button.name = nextArena.portalList[i].name;

            button.GetComponent<Button>().onClick.AddListener(nextArena.portalList[i].TriggerPortal);
        }
    }
}
