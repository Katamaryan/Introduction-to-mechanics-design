using UnityEngine;
using UnityEngine.UIElements;

public class PlayerPosition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement container = new VisualElement();
        container.name = "container";
        GetComponent<UIDocument>().rootVisualElement.Add(container);

        VisualElement labelcontainer = new VisualElement();
        labelcontainer.name = "labelcontainer";
        labelcontainer.AddToClassList("positionlabelcontainer");
        container.Add(labelcontainer);

        Label xlabel = new Label();
        xlabel.name = "xlabel";
        xlabel.text = "X: ______";
        xlabel.AddToClassList("positionlabel");
        labelcontainer.Add(xlabel);

        Label ylabel = new Label();
        ylabel.name = "ylabel";
        ylabel.text = "Y: ______";
        ylabel.AddToClassList("positionlabel");
        labelcontainer.Add(ylabel);


        TransformWrapper playerTransform = GameObject.Find("PlayerPrefab").GetComponent<TransformWrapper>();
        Debug.Log(playerTransform);
        DataBinding xBinding = new DataBinding
        {
            dataSource = playerTransform,
            dataSourcePath = new Unity.Properties.PropertyPath("XPos"),
            bindingMode = BindingMode.ToTarget
        };

        xBinding.updateTrigger = BindingUpdateTrigger.OnSourceChanged;
        xBinding.sourceToUiConverters.AddConverter((ref float value) => $"X: {value:F2}");
        xlabel.SetBinding("text", xBinding);
        
        DataBinding yBinding = new DataBinding
        {
            dataSource = playerTransform,
            dataSourcePath = new Unity.Properties.PropertyPath("YPos"),
            bindingMode = BindingMode.ToTarget
        };

        yBinding.updateTrigger = BindingUpdateTrigger.OnSourceChanged;
        yBinding.sourceToUiConverters.AddConverter((ref float value) => $"Y: {value:F2}");
        ylabel.SetBinding("text", yBinding);

    }
}
