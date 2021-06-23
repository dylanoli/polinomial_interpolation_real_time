using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    private const int SIZE_HORIZONTAL_LINES = 51;
    private const int SIZE_VERTICAL_LINES = 51;
    public GameObject StrongLineReference;
    public GameObject LightLineReference;
    public GameObject PositionTextReference;
    public Dictionary<int, GameObject> HorizontalPositions { get; set; }
    public Dictionary<int, GameObject> VerticalPositions { get; set; }
    public GameObject[] HorizontalLines { get; set; }
    public GameObject[] VerticalLines { get; set; }
    public InputField FunctionInputField { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        FunctionInputField = GameObject.Find("Canvas/FunctionInputField").GetComponent<InputField>();
        BuildLines();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClickCloseApplication()
    {
        Application.Quit();
    }
    private void BuildLines()
    {
        HorizontalPositions = new Dictionary<int, GameObject>();
        VerticalPositions = new Dictionary<int, GameObject>();
        HorizontalLines = new GameObject[SIZE_HORIZONTAL_LINES];
        VerticalLines = new GameObject[SIZE_VERTICAL_LINES];

        var horizontalAngle = Quaternion.Euler(0, 0, 90);

        for (int i = 0; i < SIZE_HORIZONTAL_LINES; i++)
        {
            int position = i - SIZE_HORIZONTAL_LINES / 2;
            if (i % 5 == 0)
            {
                HorizontalLines[i] = Instantiate(StrongLineReference, new Vector3(position, 0, 0), horizontalAngle);
                HorizontalPositions.Add(position, Instantiate(PositionTextReference, new Vector3(position, 0, 0), Quaternion.identity));
                HorizontalPositions[position].GetComponent<TMPro.TextMeshPro>().text = position.ToString();
            }
            else
                HorizontalLines[i] = Instantiate(LightLineReference, new Vector3(position, 0, 0), horizontalAngle);
        }

        for (int i = 0; i < SIZE_VERTICAL_LINES; i++)
        {
            int position = i - SIZE_VERTICAL_LINES / 2;
            if (i % 5 == 0)
            {
                VerticalLines[i] = Instantiate(StrongLineReference, new Vector3(0, position, 0), Quaternion.identity);
                VerticalPositions.Add(position, Instantiate(PositionTextReference, new Vector3(0, position, 0), Quaternion.identity));
                VerticalPositions[position].GetComponent<TMPro.TextMeshPro>().text = position.ToString();
            }
            else
                VerticalLines[i] = Instantiate(LightLineReference, new Vector3(0, position, 0), Quaternion.identity);
        }
    }
}
