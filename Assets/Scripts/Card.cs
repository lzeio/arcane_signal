using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private Image cardSprite;
    int id = -1;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Card Clicked");
    }

    public void SetCardData(Sprite sprite, int cardId)
    {
        id = cardId;
        cardSprite.sprite = sprite;
    }

}
