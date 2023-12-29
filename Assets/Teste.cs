using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Teste : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public Sprite highlightedSprite;
    public Sprite normalSprite;

    public Button button;
    public Image buttonimage;

    private void Start()
    {
        buttonimage.sprite = normalSprite;
        buttonimage = GetComponent<Image>();
        button = GetComponent<Button>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
         buttonimage.sprite = highlightedSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonimage.sprite = normalSprite;
    }

}
