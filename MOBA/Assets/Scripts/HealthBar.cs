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
        UpdatePositionAndRotation();
    }

    void Update()
    {
        transform.GetChild(1).GetComponent<Image>().fillAmount = _parent.GetComponent<Entity>().GetHealthPercent();
        UpdatePositionAndRotation();
    }

    private void UpdatePositionAndRotation()
    {
        // Adjust the position above the tower or at the parent's position
        if (_parent.tag == "Tower")
        {
            transform.position = _parent.position + new Vector3(0f, -6.0f, -1.0f);
        }
        else
        {
            transform.position = _parent.position;
        }

        // Simplify the rotation to face upwards correctly
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
