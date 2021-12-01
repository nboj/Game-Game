using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Animator), typeof(SpriteRenderer))]
public class Creature : MonoBehaviour {
    [SerializeField] private float creatureSpeed;
    [SerializeField] private Creature_SO creature_SO;
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

    public int GetDiceValue(DiceType type) {
        switch (type) {
            case DiceType.d4:
                return 4;
            case DiceType.d6:
                return 6;
            case DiceType.d8:
                return 8;
            case DiceType.d10:
                return 10;
            case DiceType.d12:
                return 12;
        }
        return -1;
    }

    public Creature_SO CreatureSO {
        get => creature_SO;
        set => creature_SO = value;
    }
    public float RollDie(int max) {
        return Random.Range(0, max) + 1;
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