using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : CharacterBase
{
    public static PlayerCondition Instance { get; private set; }
    public enum State { IDLE, DEFENDING, BUSY };
    public State _state = State.IDLE;

    new void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

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
    }
}
