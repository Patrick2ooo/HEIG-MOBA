[RealtimeModel]
public partial class DamageModel
{
    [RealtimeProperty(1, true)] private string _target;
    [RealtimeProperty(2, true)] private string _attacker;
    [RealtimeProperty(3, true)] private float _physDamage;
    [RealtimeProperty(4, true)] private float _magDamage;
    [RealtimeProperty(5, true)] private float _physPen;
    [RealtimeProperty(6, true)] private float _magPen;
    [RealtimeProperty(7, true)] private float _critChance;
    [RealtimeProperty(8, true)] private float _critMult;
}
