using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class CravacheSevere : Item
{
    public override int GetAttack() {
        return 50;
    }

    public override string GetName() {
        return "Cravache Sévère";
    }
}
