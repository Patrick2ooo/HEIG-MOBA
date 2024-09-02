using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class GemmeDeFeu : Item
{
    public override int GetHealth() {
        return 500;
    }

    public override uint GetCost() {
        return 550;
    }

    public override string GetName() {
        return "Gemme de Feu";
    }
}
