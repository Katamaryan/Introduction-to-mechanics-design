using UnityEngine;
using UnityEngine.UIElements;
using System;
using Random = UnityEngine.Random;

public class DynamicMenu : MonoBehaviour
{
    private VisualElement m_uiRoot;
    private ScrollView m_scrollArea;
    void Awake()
    {
        VisualElement rootElement = new VisualElement();
        rootElement.name = "container";

        GetComponent<UIDocument>().rootVisualElement.Add(rootElement);
        m_uiRoot = rootElement;


        Button button = new Button();
        button.text = "Press me!";
        button.name = "btn_labelspawner";
        button.RegisterCallback<ClickEvent>(Handle_LabelSpawn);
        button.AddToClassList("button");
        m_uiRoot.Add(button);
    }

    private void Handle_LabelSpawn(ClickEvent clickEvent)
    {
        if (m_scrollArea == null)
        {
            ScrollView list = new ScrollView();
            list.name = "scr-labelcontainer";
            list.AddToClassList("scrollview");

            m_uiRoot.Add(list);
            m_scrollArea = list;

        }
        Label label = new Label();
        label.name = "lb_randomnumberlabel";
        label.text = Random.Range(Int32.MinValue, Int32.MaxValue).ToString();
        label.AddToClassList("label");
        m_scrollArea.Add(label);
    }

    
}
