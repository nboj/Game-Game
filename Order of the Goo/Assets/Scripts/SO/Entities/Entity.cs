using UnityEngine;

public class Entity : ScriptableObject {
    [SerializeField] private int id;
    [SerializeField] private bool canPickup;
    [SerializeField] private bool canDrop;
    [SerializeField] private float pickupRadius;
    [SerializeField] private string description;

    public int ID {
        get => id;
    }

    public bool CanPickup {
        get => canPickup;
    }

    public bool CanDrop {
        get => canDrop;
    }

    public float PickupRadius {
        get => pickupRadius;
    }

    public string Description {
        get => description;
    }
}