using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    protected Transform Parent;

    void Start()
    {
        Parent = transform.parent.transform;
        transform.GetChild(1).GetComponent<Image>().fillAmount = Parent.GetComponent<Entity>().GetHealthPercent();
        transform.position = Parent.position;
        transform.rotation = Quaternion.Inverse(Parent.rotation);
        transform.rotation = Quaternion.AngleAxis(60, Vector3.right);
    }

    void Update()
    {
        transform.GetChild(1).GetComponent<Image>().fillAmount = Parent.GetComponent<Entity>().GetHealthPercent();
        transform.position = Parent.position;
        transform.rotation = Quaternion.Inverse(Parent.rotation);
        transform.rotation = Quaternion.AngleAxis(60, Vector3.right);
    }
}
