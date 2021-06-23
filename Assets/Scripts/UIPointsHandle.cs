using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIPointsHandle : MonoBehaviour
{
    public const int INDEX_POINT_NAME = 1;
    public const int INDEX_X_INPUT_FIELD = 2;
    public const int INDEX_Y_INPUT_FIELD = 3;
    public const int INDEX_DELETE_BUTTOM = 4;
    public PointsHandle PointHandleScript { get; set; }
    public GameObject UIPointReference;
    public Transform Content { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        PointHandleScript = GetComponent<PointsHandle>();
        Content = GameObject.Find("Canvas/GroundPoints/Scroll View/Viewport/Content").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject AddPointUI(float xPosition, float yPosition, int id,
        Func<int, int> removeFunction,
        Func<int, string, string, int> changePositionFunction
    )
    {
        var position = new Vector2(170, -PointHandleScript.RedPoints.Count * 30 - 16);
        var gameObject = Instantiate(UIPointReference, Vector3.zero, Quaternion.identity, Content);
        var transform = gameObject.GetComponent<Transform>();

        transform.localPosition = position;
        transform.GetChild(INDEX_POINT_NAME).GetComponent<Text>().text = "Point " + id;
        transform.GetChild(INDEX_X_INPUT_FIELD).GetComponent<InputField>().text = xPosition.ToString();
        transform.GetChild(INDEX_Y_INPUT_FIELD).GetComponent<InputField>().text = yPosition.ToString();

        transform.GetChild(INDEX_DELETE_BUTTOM).GetComponent<Button>().onClick.AddListener(delegate { removeFunction(id); });

        UnityAction<string> functionChangePositionFinal =
            delegate
            {
                changePositionFunction(id,
                transform.GetChild(INDEX_X_INPUT_FIELD).GetComponent<InputField>().text,
                transform.GetChild(INDEX_Y_INPUT_FIELD).GetComponent<InputField>().text);
            };
        transform.GetChild(INDEX_X_INPUT_FIELD).GetComponent<InputField>().onValueChanged.AddListener(functionChangePositionFinal);
        transform.GetChild(INDEX_Y_INPUT_FIELD).GetComponent<InputField>().onValueChanged.AddListener(functionChangePositionFinal);

        return gameObject;
    }
}
