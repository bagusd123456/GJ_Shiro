using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public bool active;
    
    public List<Collider2D> colliderList = new List<Collider2D>();
    
    public PolygonCollider2D polygonCollider;

    // Start is called before the first frame update
    void Start()
    {
        //if(collider == null)
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
            if (colliderList[i].GetComponent<CapsuleCollider2D>() == null)
                colliderList[i].enabled = active;
        }
    }

    public void ToggleSelectablePortal(bool T)
    {
        active = T;

        /*if(T == true)
            for (int a = 0; a < colliderList.Count; a++)
            {
                colliderList[a].GetComponent<Collider2D>().enabled = true;
            }
        else
            for (int b = 0; b < colliderList.Count; b++)
            {
                colliderList[b].GetComponent<Collider2D>().enabled = false;
            }*/
    }
}
