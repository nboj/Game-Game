using Pathfinding;
using UnityEngine;
using RPG.Saving;
using System.Collections.Generic;

public class Creature : MonoBehaviour, ISaveable {
    [SerializeField] private float creatureSpeed;
    [SerializeField] private Creature_SO creature_SO;
    [SerializeField] bool canControl = true;
    [SerializeField] private bool startDisabled = false;
    private Health health;
    private Animator animator;
    private RigidbodyMovement rigidbodyMovement;
    private AStarMovement aStarMovement;

    public virtual void Awake() { 
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        CanControl = canControl;
        AIPath path = GetComponent<AIPath>(); 
        aStarMovement = new AStarMovement(creatureSpeed, path, animator, this); 
        rigidbodyMovement = new RigidbodyMovement(CreatureSpeed, GetComponent<Rigidbody2D>(), animator); 
    }

    public virtual void Start() { 
    }

    public virtual void OnEnable() { 
        if (startDisabled) {
            SetEnabled(false);
        }
    }


    public virtual void Update() {
    }

    public virtual void FixedUpdate() {
    }

    public Creature_SO CreatureSO {
        get => creature_SO;
        set => creature_SO = value;
    }

    public RigidbodyMovement RigidbodyMovement => rigidbodyMovement;

    public AStarMovement ASMovement => aStarMovement;
    
    public bool CanControl {
        get => canControl;
        set => canControl = value;
    }

    protected internal Health Health {
        get => health; 
    }

    protected internal Animator Animator {
        get => animator;
        set => animator = value;
    }

    protected virtual internal void SetEnabled(bool value) {
        CanControl = value;
        if (value) {
            health.Enable();
        } else {
            health.Disable();
        }
    }

    object ISaveable.CaptureState() {
        var dict = new Dictionary<string, object>();
        dict["canControl"] = CanControl;
        dict["creatureSpeed"] = CreatureSpeed;
        dict["startDisabled"] = startDisabled;
        dict["serializedPosition"] = new SerializableVector3(transform.position);
        return dict; 
    }

    void ISaveable.RestoreState(object state) {
        var dict = (Dictionary<string, object>)state;
        CanControl = (bool)dict["canControl"];
        creatureSpeed = (float)dict["creatureSpeed"];
        startDisabled = (bool)dict["startDisabled"];
        transform.position = ((SerializableVector3)dict["serializedPosition"]).ToVector();
        Debug.Log("Updating pos in creature");
    }

    protected internal float CreatureSpeed => creatureSpeed; 
}