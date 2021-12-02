using UnityEngine;

public class Entity : ScriptableObject, ISerializationCallbackReceiver {
    [SerializeField] private string id;
    [SerializeField] private bool canPickup;
    [SerializeField] private bool canDrop;
    [SerializeField] private float pickupRadius;
    [SerializeField] private string description;
    [SerializeField] private float weight;
    [SerializeField] private Sprite sprite;

    public Sprite Sprite => sprite;

    public string ID {
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

    public float Weight => weight;

    void ISerializationCallbackReceiver.OnAfterDeserialize() {
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize() {
        if (string.IsNullOrEmpty(ID)) {
            id = System.Guid.NewGuid().ToString();
        }
    }
}