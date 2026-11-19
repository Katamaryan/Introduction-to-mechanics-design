
using UnityEngine;
using UnityEngine.UIElements;

public class TimerUI : MonoBehaviour
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

        Label timer = new Label();
        timer.name = "timer";
        timer.text = "Time: ____";
        timer.AddToClassList("positionlabel");
        labelcontainer.Add(timer);
        
        Label highscore = new Label();
        highscore.name = "highscore";
        highscore.text = "High: ____";
        highscore.AddToClassList("positionlabel");
        labelcontainer.Add(highscore);

        TimerWrapper timerWrapper = GameObject.FindWithTag("Timer").GetComponent<TimerWrapper>();
        DataBinding currentBinding = new DataBinding
        {
            dataSource = timerWrapper,
            dataSourcePath =  new Unity.Properties.PropertyPath("currentTime"),
            bindingMode = BindingMode.ToTarget
        };

        currentBinding.updateTrigger = BindingUpdateTrigger.OnSourceChanged;
        currentBinding.sourceToUiConverters.AddConverter((ref float value) => $"Time: {value:F2}");
        timer.SetBinding("text", currentBinding);
        
        DataBinding highBinding = new DataBinding
        {
            dataSource = timerWrapper,
            dataSourcePath =  new Unity.Properties.PropertyPath("highScore"),
            bindingMode = BindingMode.ToTarget
        };

        highBinding.updateTrigger = BindingUpdateTrigger.OnSourceChanged;
        highBinding.sourceToUiConverters.AddConverter((ref float value) => $"High: {value:F2}");
        highscore.SetBinding("text", highBinding);

    }

}
