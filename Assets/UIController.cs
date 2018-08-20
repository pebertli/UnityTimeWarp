using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject cubes;
    public Button button;
    private
        Slider slider;
    RotatorCubes rotator;

    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnValueChange);
        rotator = cubes.GetComponent<RotatorCubes>();
        ResetCubes();
    }

    public void ActionButtonClick()
    {
        if (rotator.rotating)
        {
            rotator.rotating = false;
            slider.interactable = true;
            button.gameObject.transform.Find("Text").GetComponent<Text>().text = "Reset";
            foreach (Transform child in cubes.transform)
            {
                child.GetComponent<Rigidbody>().useGravity = true;
                child.GetComponent<Rigidbody>().isKinematic = false;
                child.GetComponent<TimeWarp>().ResetRecords();
                child.GetComponent<TimeWarp>().isWarping = false;
            }
        }
        else
        {
            slider.interactable = false;
            ResetCubes();
        }
    }

    public void ResetCubes()
    {
        button.gameObject.transform.Find("Text").GetComponent<Text>().text = "Action";
        cubes.transform.rotation = Quaternion.identity;
        for (int k = 0; k < 2; k++)
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {

                    Transform child = cubes.transform.GetChild((k * 9) + (i * 3) + j);
                    child.localRotation = Quaternion.identity;
                    child.localPosition = new Vector3(((i-1) * 2)+k, k * 2, (j-1) * 2);
                    child.GetComponent<Rigidbody>().useGravity = false;
                    child.GetComponent<Rigidbody>().isKinematic = true;
                    child.GetComponent<TimeWarp>().isWarping = true;                    
                }

        rotator.rotating = true;
    }

    public void OnValueChange(float value)
    {
        foreach (Transform child in cubes.transform)
        {
            Debug.Log(value);
            child.GetComponent<TimeWarp>().SetPointInTime(value);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (Transform child in cubes.transform)
        {
            child.GetComponent<TimeWarp>().StartWarping();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (Transform child in cubes.transform)
        {
            child.GetComponent<TimeWarp>().StopWarping();
        }
        slider.value = slider.maxValue;
    }
}
