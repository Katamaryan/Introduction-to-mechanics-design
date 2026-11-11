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
        
        Label xVollabel = new Label();
        xVollabel.name = "xVollabel";
        xVollabel.text = "X velocity: ______";
        xVollabel.AddToClassList("positionlabel");
        labelcontainer.Add(xVollabel);
        
        Label yVollabel = new Label();
        yVollabel.name = "yVollabel";
        yVollabel.text = "Y velocity: ______";
        yVollabel.AddToClassList("positionlabel");
        labelcontainer.Add(yVollabel);


        TransformWrapper playerTransform = GameObject.Find("PlayerPrefab(Clone)").GetComponent<TransformWrapper>();
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

        DataBinding xVolBinding = new DataBinding
        {
            dataSource = playerTransform,
            dataSourcePath = new Unity.Properties.PropertyPath("XVol"),
            bindingMode = BindingMode.ToTarget
        };

        xVolBinding.updateTrigger = BindingUpdateTrigger.OnSourceChanged;
        xVolBinding.sourceToUiConverters.AddConverter((ref float value) => $"X velocity: \n{value:F2}");
        xVollabel.SetBinding("text", xVolBinding);

        DataBinding yVolBinding = new DataBinding
        {
            dataSource = playerTransform,
            dataSourcePath = new Unity.Properties.PropertyPath("YVol"),
            bindingMode = BindingMode.ToTarget
        };

        yVolBinding.updateTrigger = BindingUpdateTrigger.OnSourceChanged;
        yVolBinding.sourceToUiConverters.AddConverter((ref float value) => $"X velocity: \n{value:F2}");
        yVollabel.SetBinding("text", yVolBinding);
    }
}
