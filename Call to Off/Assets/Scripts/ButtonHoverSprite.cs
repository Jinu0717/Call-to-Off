using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverSprite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("버튼 이미지")]
    [SerializeField] private Image targetImage;

    [Header("기본 스프라이트")]
    [SerializeField] private Sprite normalSprite;

    [Header("마우스 올렸을 때 스프라이트")]
    [SerializeField] private Sprite hoverSprite;

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (targetImage != null && normalSprite != null)
            targetImage.sprite = normalSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage == null) return;
        if (hoverSprite == null) return;

        targetImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage == null) return;
        if (normalSprite == null) return;

        targetImage.sprite = normalSprite;
    }
}