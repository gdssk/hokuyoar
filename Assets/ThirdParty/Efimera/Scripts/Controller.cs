using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    /******* UI ******/
    [SerializeField] private InputField ifPrefabName;
    
    [SerializeField] private GameObject container;
    [SerializeField] private int index;
    [SerializeField] private Slider sliderTimeScale;

    private GameObject[] elementsArray;

    void Start()
    {
        elementsArray = new GameObject[container.transform.childCount];
 print ("elemento " + elementsArray.Length);
        for (int i = 0; i < elementsArray.Length; i++)
        {
            elementsArray[i] = container.transform.GetChild(i).gameObject;
            elementsArray[i].gameObject.SetActive(false);
        }

        elementsArray[index].gameObject.SetActive(true);

        sliderTimeScale.onValueChanged.AddListener(delegate {ValueChangeCheck(); });

        UpdateUI();
    }

    public void Next()
    {
        elementsArray[index].gameObject.SetActive(false);

        index++;

        if (index > elementsArray.Length - 1) index = 0;

        elementsArray[index].gameObject.SetActive(true);

        UpdateUI();
    }

    public void Prev()
    {
        elementsArray[index].gameObject.SetActive(false);

        index--;

        if (index < 0) index = elementsArray.Length - 1;

        elementsArray[index].gameObject.SetActive(true);

        UpdateUI();
    }

    public void ValueChangeCheck()
    {print ("ValueChangeCheck value " + sliderTimeScale.value);
        Time.timeScale = sliderTimeScale.value;
    }

    private void UpdateUI()
    {
        ifPrefabName.text = elementsArray[index].gameObject.name;
    }
}
