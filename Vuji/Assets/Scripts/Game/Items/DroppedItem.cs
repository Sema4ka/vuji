using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public BaseItem itemData;

    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = itemData.GetImage();
    }

    public void SetItem(BaseItem aitemData)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        this.itemData = aitemData;
        _spriteRenderer.sprite = aitemData.GetImage();
    }
    void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.gameObject.CompareTag("Player"))
        {
            entity.gameObject.GetComponent<Inventory>().AddItem(itemData, gameObject);
            Destroy(gameObject);
        }
    }
}