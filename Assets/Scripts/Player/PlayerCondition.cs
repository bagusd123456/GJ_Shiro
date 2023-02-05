using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : CharacterBase
{
    public static PlayerCondition Instance { get; private set; }
    public enum State { IDLE, DEFENDING, BUSY };
    public State _state = State.IDLE;

    public float damageTakenCD;
    float damageTakenTime;

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

        if (damageTakenTime > 0)
        {
            damageTakenTime -= Time.deltaTime;
        }
            
        if(isDead == true)
        {
            SceneLoad.Instance.MatiCuk();
        }
    }

    public override void TakeDamage(int damage)
    {
        if(_state != State.DEFENDING)
        {
            if (damageTakenTime <= 0)
            {
                base.TakeDamage(damage);
                damageTakenTime = damageTakenCD;
            }
        }
            
    }
}
