using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static ushort PlayerSide;
    private Transform _parent;

    void Start()
    {
        _parent = transform.parent.transform;
        transform.GetChild(1).GetComponent<Image>().color = _parent.GetComponent<Entity>().GetSide() == PlayerSide ? Color.green : Color.red;

        transform.GetChild(1).GetComponent<Image>().fillAmount = _parent.GetComponent<Entity>().GetHealthPercent();
        transform.position = _parent.position;
        transform.rotation = Quaternion.Inverse(_parent.rotation);
        transform.rotation = Quaternion.AngleAxis(60, Vector3.right);
    }

    void Update()
    {
        transform.GetChild(1).GetComponent<Image>().fillAmount = _parent.GetComponent<Entity>().GetHealthPercent();
        transform.position = _parent.position;
        transform.rotation = Quaternion.Inverse(_parent.rotation);
        transform.rotation = Quaternion.AngleAxis(60, Vector3.right);
    }
}
