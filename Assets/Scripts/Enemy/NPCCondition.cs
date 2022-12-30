using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCondition : CharacterBase
{
    new void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if(isDead)
            Destroy(gameObject);
    }
}
