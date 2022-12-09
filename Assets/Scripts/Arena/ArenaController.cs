using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public bool active;
    //public bool enemyActive;
    //public CircleCollider2D collider;
    public List<CircleCollider2D> colliderList = new List<CircleCollider2D>();
    
    public PolygonCollider2D polygonCollider;
    //public List<CapsuleCollider2D> enemyColliderList = new List<CapsuleCollider2D>();

    // Start is called before the first frame update
    void Start()
    {
        //if(collider == null)
        polygonCollider = GetComponentInChildren<PolygonCollider2D>();

        colliderList = GetComponentsInChildren<CircleCollider2D>().ToList();

        //enemyColliderList = GetComponentsInChildren<CapsuleCollider2D>().ToList();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (polygonCollider != null)
            polygonCollider.enabled = active;
        
        for (int i = 0; i < colliderList.Count; i++)
        {
            colliderList[i].enabled = active;
        }
        /*
        for (int i = 0; i < enemyColliderList.Count; i++)
        {
            enemyColliderList[i].enabled = active;
        }
        */
    }
}
