using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public struct UiItems
{
    public GameObject Item;
    public BoolReference IsActive;
}
public class LabelPositionManager : MonoBehaviour
{
    [SerializeField] private HorizontalDrawer drawer;
    [SerializeField] private List<UiItems> uiItems;
    [SerializeField] private float spacingTop;
    [SerializeField] private float startY;
    private int nbOfItemsActive;

    void Start()
    {
        float showingY = 0;
        nbOfItemsActive = 0;
        for (int i = 0; i < uiItems.Count; i++)
        {
            UiItems UIitem = uiItems[i];
            if (UIitem.IsActive.Value) {
                //showingY -= (UIitem.Item.transform as RectTransform).rect.height;
                Vector3 currentPos = UIitem.Item.transform.localPosition;
                float newY = startY + spacingTop*nbOfItemsActive;
                UIitem.Item.transform.localPosition = new Vector3(currentPos.x, currentPos.y+newY, currentPos.z);
                nbOfItemsActive++;
            }
            else
            {
                UIitem.Item.SetActive(false);
            }
        }

        if (nbOfItemsActive==0)
        {
            showingY = -50;
            gameObject.SetActive(false);
        }
        else
        {
            showingY += -(startY+spacingTop*nbOfItemsActive);
        }

        drawer.SetShowingY(showingY);
    }
}
