using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyArrowPlayer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image button;
    [SerializeField] private Color onPressed;
    [SerializeField] private KeyCode key;
    [SerializeField] private UnityEvent onPressedEvents;
    private Color startColor;
    private bool pointerIsDown;

    void Start()
    {
        startColor = button.color;
    }

    void Update()
    {
        if (pointerIsDown)
        {
            onPressedEvents?.Invoke();
        }
        else
        {
            if (Input.GetKeyDown(key))
            {
                ChangeColorOnPointerDown();
            }
            if (Input.GetKeyUp(key))
            {
                ChangeColorOnPointerUp();
            }
        }
    }
    public void ChangeColorOnPointerDown()
    {
        button.color = onPressed;
    }

    public void ChangeColorOnPointerUp()
    {
        button.color = startColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerIsDown = true;
        ChangeColorOnPointerDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerIsDown = false;
        ChangeColorOnPointerUp();
    }
}
