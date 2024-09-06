using UnityEngine.SceneManagement;

public class Nexus : Entity
{
    protected override int GetGoldBounty()
    {
        return 0;
    }

    protected override int GetExpBounty()
    {
        return 0;
    }

    protected override void SetValues(Attributes attributes)
    {
        attributes.maxHealth = 5500;   
        attributes.health = 5500;
        attributes.healthRegen = 20;
    }

    protected override void Update()
    {
        base.Update();
        if (model.health <= 0)
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
