using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityNameManager : MonoBehaviour
{
    [SerializeField] Text entityName;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = Camera.main.WorldToScreenPoint(transform.parent.parent.position + offset);
        entityName.transform.position = new Vector3(temp.x, temp.y + 20, 0);
    }

    public void SetText(string newText)
    {
        entityName.text = newText;
    }
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
    public Vector3 GetOffset()
    {
        return offset;
    }
}
