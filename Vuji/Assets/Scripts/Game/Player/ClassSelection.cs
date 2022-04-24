using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelection : MonoBehaviour
{
    [SerializeField] GameObject classSelection;

    [SerializeField] private Text firstClassName;
    [SerializeField] private Text secondClassName;
    [SerializeField] private Text thirdClassName;

    [SerializeField] private RectTransform firstClass;
    [SerializeField] private RectTransform secondClass;
    [SerializeField] private RectTransform thirdClass;

    [SerializeField] private GameObject[] firstClassSubclasses;
    [SerializeField] private GameObject[] secondClassSubclasses;
    [SerializeField] private GameObject[] thirdClassSubclasses;

    [SerializeField] private RectTransform firstClassSubclassesPanel;
    [SerializeField] private RectTransform secondClassSubclassesPanel;
    [SerializeField] private RectTransform thirdClassSubclassesPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
