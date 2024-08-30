using System.Collections.Generic;
using Scripts;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class Attributes
{
    // constants
    public const int MaxLevel = 12, PassiveGold = 1, PassiveExp = 1, NbInventorySlots = 6;
    
    // local variables
    public float PassiveIncomeTimer = 0, RegenTimer = 0;
    
    public readonly Stack<Character> LastHitters = new();
    public Entity Target;
    
    // synched variables
    [RealtimeProperty(1, true, true)] private float _health = 1;
    [RealtimeProperty(2, false, true)] private ushort _side;
    [RealtimeProperty(3, true, true)] private int _physDef;
    [RealtimeProperty(4, true, true)] private int _magDef;
    [RealtimeProperty(5, true, true)] private int _level = 1;
    [RealtimeProperty(6, false, true)] private int _physDefPerLevel;
    [RealtimeProperty(7, false, true)] private int _magDefPerLevel;
    [RealtimeProperty(8, false, true)] private int _golds;
    [RealtimeProperty(9, true, true)] private int _exp;
    [RealtimeProperty(10, false, true)] private int _core;
    [RealtimeProperty(11, false, true)] private int _playerID;
    [RealtimeProperty(12, false, true)] private float _attack;
    [RealtimeProperty(13, false, true)] private float _attackRange;
    [RealtimeProperty(14, true, true)] private float _maxHealth = 1;
    [RealtimeProperty(15, false, true)] private float _windUpTime;
    [RealtimeProperty(16, false, true)] private float _attackTime;
    [RealtimeProperty(17, false, true)] private float _recoveryTime;
    [RealtimeProperty(18, false, true)] private float _attackPerLevel;
    [RealtimeProperty(19, false, true)] private float _healthPerLevel;
    [RealtimeProperty(20, false, true)] private float _healthRegen;
    [RealtimeProperty(21, false, true)] private float _healthRegenPerLevel;
    [RealtimeProperty(22, false, true)] private float _attackSpeed;
    [RealtimeProperty(23, false, true)] private float _haste;
    [RealtimeProperty(24, false, true)] private float _critChance;
    [RealtimeProperty(25, false, true)] private float _critMult = 1.5f;
    [RealtimeProperty(26, false, true)] private float _physPen;
    [RealtimeProperty(27, false, true)] private float _magPen;
    [RealtimeProperty(28, false, true)] private float _physLifeSteal;
    [RealtimeProperty(29, false, true)] private float _magLifeSteal;
    [RealtimeProperty(30, false, true)] private float _mana;
    [RealtimeProperty(31, false, true)] private float _manaPerLevel;
    [RealtimeProperty(32, false, true)] private float _manaRegen;
    [RealtimeProperty(33, false, true)] private float _manaRegenPerLevel;
    [RealtimeProperty(34, true, true)] private float _moveSpeed;
    [RealtimeProperty(35, false, true)] private string _name;
    [RealtimeProperty(36, true, true)] private RealtimeDictionary<Item> _inventory;
    [RealtimeProperty(37, false, true)] private float _radius;
}
