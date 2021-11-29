using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Animator), typeof(SpriteRenderer))]
public class Creature : MonoBehaviour {
    [SerializeField] private float creatureSpeed;
    private bool canControl;
    private Health health;
    private Animator animator;
    private RigidbodyMovement rigidbodyMovement;
    private AStarMovement aStarMovement;

    public virtual void Start() { 
        AIPath path = GetComponent<AIPath>(); 
        aStarMovement = new AStarMovement(creatureSpeed, path); 
        rigidbodyMovement = new RigidbodyMovement(CreatureSpeed, GetComponent<Rigidbody2D>()); 
    }

    public virtual void Update() {
    }

    public virtual void FixedUpdate() {
    }
    
    public RigidbodyMovement RigidbodyMovement => rigidbodyMovement;

    public AStarMovement ASMovement => aStarMovement;
    
    public bool CanControl => canControl;

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

    private void Awake() {
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        canControl = true;
    }
}