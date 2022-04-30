using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public BaseItem itemData;

    private SpriteRenderer _spriteRenderer;

    public void SetItem(BaseItem aitemData)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        this.itemData = aitemData;
        _spriteRenderer.sprite = aitemData.GetImage();
    }

}