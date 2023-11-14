using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Dice : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    RectTransform rect;
    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public void Use()
    {
        gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        rect.DOAnchorPos(rect.anchoredPosition + new Vector2(Random.Range(-10, 10f), Random.Range(-10, 10f)), Random.Range(0.5f, 1));
        rect.DORotate(new Vector3(0, 0, Random.Range(-30, 30f)), Random.Range(0.5f, 1));
    }
}
