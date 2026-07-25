using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PowerupTimer : MonoBehaviour
{
    [SerializeField] RawImage icon;
    [SerializeField] Image background;

    [SerializeField] private PowerupIcon PowerupIcon;

    private void Start()
    {
        icon.texture = PowerupIcon.icon;
        background.color = PowerupIcon.color;
    }

    public void SetPercentageTime(float value)
    {
        if (value <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        background.fillAmount = value;
    }
}
