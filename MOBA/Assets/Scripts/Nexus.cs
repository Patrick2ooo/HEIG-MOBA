using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nexus : Entity
{

    public Image ImgHealthBar;

    public override int GetGoldBounty()
    {
        return 0;
    }

    public override int GetExpBounty()
    {
        return 0;
    }

    void Start()
    {
        MaxHealth = 100;   
        Health = 100;
    }

    void Update()
    {
        ImgHealthBar.fillAmount = GetHealthPercent();
    }
}
