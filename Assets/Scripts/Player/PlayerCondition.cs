using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : CharacterBase
{
    public enum State { IDLE, DEFENDING, BUSY };
    public State _state = State.IDLE;

    private void Awake()
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
    }
}
