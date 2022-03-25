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

    void OnTriggerEnter2D(Collider2D entity)
    {
        
        if (entity.gameObject.CompareTag("Player"))
        {
            entity.gameObject.GetComponent<Inventory>().AddItem(itemData);

            Destroy(gameObject);
        }
    }
}