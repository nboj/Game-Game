using Pathfinding;
using UnityEngine;
 
public class Creature : MonoBehaviour {
    [SerializeField] private float creatureSpeed;
    [SerializeField] private Creature_SO creature_SO;
    [SerializeField] bool canControl = true;
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
        health.enabled = value; 
    }

    protected internal float CreatureSpeed => creatureSpeed; 
}