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

    void OnTriggerEnter2D(Collider2D entitiy)
    {
        
        if (entitiy.gameObject.CompareTag("Player"))
        {
            entitiy.gameObject.GetComponent<Inventory>().AddItem(itemData);

            Destroy(gameObject);
        }
    }
}
