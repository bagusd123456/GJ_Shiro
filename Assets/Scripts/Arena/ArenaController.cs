using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public static ArenaController Instance { get; private set; }

    public bool active;
    
    public List<Collider2D> colliderList = new List<Collider2D>();
    
    public PolygonCollider2D polygonCollider;

    public List<Portal> portalList = new List<Portal>();

    private void Awake()
    {
        List<Portal> portal = GetComponentsInChildren<Portal>().ToList();

        for (int i = 0; i < portal.Count; i++)
        {
            if (portal[i].GetComponent<Portal>()._portalType == Portal.PortalType.BUSWAY_EXIT)
                portalList.Add(portal[i]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        polygonCollider = GetComponentInChildren<PolygonCollider2D>();
        colliderList = GetComponentsInChildren<Collider2D>().ToList();

        for (int i = 0; i < colliderList.Count; i++)
        {
            if (colliderList[i].GetComponent<PlayerCondition>() != null)
                colliderList.Remove(colliderList[i]);
            else if (colliderList[i].GetComponent<NPCMovement>() != null)
                colliderList.Remove(colliderList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (polygonCollider != null)
            polygonCollider.enabled = active;

        for (int i = 0; i < colliderList.Count; i++)
        {
            if (colliderList[i] != null)
            {
                if (colliderList[i].GetComponent<CapsuleCollider2D>() == null)
                    colliderList[i].enabled = active;
            }
        }
    }

    public void ToggleSelectablePortal(bool T)
    {
        active = T;
    }
}
