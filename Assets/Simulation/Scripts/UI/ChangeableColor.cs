using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeableColor : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private List<Color> colors;
    private Color defaultColor;

    void Start()
    {
        defaultColor = image.color;
    }

    public void SetColor(int id)
    {
        if (id >= colors.Count)
        {
            return;
        }
        image.color = colors[id];
    }

    public void ResetColor()
    {
        image.color = defaultColor;
    }
}
