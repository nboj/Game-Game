using UnityEngine;

public enum CreatureClass { 
    ARTIFICER,   
    BARBARIAN,  
    BARD,    
    CLERIC, 
    DRUID,  
    FIGHTER, 
    MONK,   
    PALADIN, 
    RANGER,  
    ROGUE,   
    SORCERER,    
    WARLOCK, 
    WIZARD  
}

public enum DiceType {
    d4,
    d6,
    d8,
    d10,
    d12
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
                case CreatureClass.WARLOCK:
                    return DiceType.d8;
                case CreatureClass.SORCERER:
                    return DiceType.d6;
                case CreatureClass.ROGUE:
                    return DiceType.d8;
                case CreatureClass.RANGER:
                    return DiceType.d10;
                case CreatureClass.PALADIN:
                    return DiceType.d10;
                case CreatureClass.MONK:
                    return DiceType.d8;
                case CreatureClass.FIGHTER:
                    return DiceType.d10;
                case CreatureClass.DRUID:
                    return DiceType.d8;
                case CreatureClass.CLERIC:
                    return DiceType.d8;
                case CreatureClass.BARD:
                    return DiceType.d8;
                case CreatureClass.BARBARIAN:
                    return DiceType.d12;
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
