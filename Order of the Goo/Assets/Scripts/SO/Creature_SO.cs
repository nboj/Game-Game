using UnityEngine;

public enum CreatureClass {  
    FIGHTER, // Heavy Melee 
    RANGER, // Ranged
    ROGUE, // Light Melee
    WIZARD  // Magic
}

public enum DiceType { 
    d4,
    d6,
    d8,
    d10,
    d12,
    d20
}

[CreateAssetMenu(menuName = "Creature", fileName = "New Creature")]
public class Creature_SO : ScriptableObject {
    [Header("Creature Properties")]
    [SerializeField] private CreatureClass creatureClass; 
    [SerializeField] private float strength = 10f;
    [SerializeField] private float dexterity = 10f;
    [SerializeField] private float intelligence = 10f;
    [SerializeField] private float wisdom = 10f;
    [SerializeField] private float charisma = 10f;
    [SerializeField] private float constitution = 10f;
    [SerializeField] private float ac = 0f;
    [SerializeField] private float level = 1f;

    public CreatureClass CreatureClass => creatureClass;
    public DiceType DiceType {
        get {
            switch (creatureClass) {
                case CreatureClass.WIZARD:
                    return DiceType.d6;  
                case CreatureClass.ROGUE:
                    return DiceType.d8;
                case CreatureClass.RANGER:
                    return DiceType.d10; 
                case CreatureClass.FIGHTER:
                    return DiceType.d10; 
                default:
                    return DiceType.d8; 
            } 
        }
    }
    public float Strength => strength;
    public float Dexterity => dexterity;    
    public float Intelligence => intelligence;
    public float Wisdom => wisdom;
    public float Charisma => charisma;
    public float Constitution => constitution;
    public float AC => ac;
    public float Level => level;
} 
