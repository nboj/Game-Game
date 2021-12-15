using UnityEngine;

public static class GameController { 
    public static float RollDie(float multiplier, int max) {
        var totalAmount = 0;
        for (var i = 0; i < multiplier; i++) {
            totalAmount += Random.Range(0, max) + 1;
        }
        return totalAmount;
    } 

    public static int GetDiceValue(DiceType type) {
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

    public static int GetModifierValue(CreatureClass creatureClass) {
        return GetDiceValue(GetClassDie(creatureClass)) / 2; 
    } 

    public static DiceType GetClassDie(CreatureClass creatureClass) {
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