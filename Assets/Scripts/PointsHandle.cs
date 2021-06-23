using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsHandle : MonoBehaviour
{
    public GameObject RedPointReference;
    public GameObject BluePointReference;
    public UIPointsHandle UIPointsHandleScript { get; set; }
    public Background BackgroundScript { get; set; }
    public NewtonInterpolate NewtonInterpolateScript { get; set; }
    public Dictionary<int, RedPoint> RedPoints { get; set; } = new Dictionary<int, RedPoint>();
    public Dictionary<int, Point> BluePoints { get; set; } = new Dictionary<int, Point>();
    private Transform _targetToMove;
    private int _topReferenceId = 0;
    public const int COUNT_BLUE_POINTS = 101;
    // Start is called before the first frame update
    void Start()
    {
        UIPointsHandleScript = GetComponent<UIPointsHandle>();
        NewtonInterpolateScript = GetComponent<NewtonInterpolate>();
        BackgroundScript = GetComponent<Background>();

        var xFirst = 0;
        var yFirst = 0;
        AddRedPoint(xFirst, yFirst);

        var xSecond = 3;
        var ySecond = 0;
        AddRedPoint(xSecond, ySecond);

        BuildBlueLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "RedPoint")
                {
                    _targetToMove = hit.transform;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            _targetToMove = null;
        }
        if (_targetToMove != null)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int id = int.Parse(_targetToMove.name.Substring(5));
            RedPoints[id].ChangePosition(position.x, position.y);
        }
    }

    public void OnAddPointButtom()
    {
        AddRedPoint(1, 1);
    }

    void AddRedPoint(float xPosition, float yPosition)
    {
        int id = GenerateNewId();
        var pointFirst = Instantiate(RedPointReference, new Vector3(xPosition, yPosition, -0.5f), Quaternion.identity);
        var uiPointFirst = UIPointsHandleScript.AddPointUI(xPosition, yPosition, id, RemoveRedPoint, ChangePositionByUI);
        RedPoints.Add(id, new RedPoint(id, pointFirst, uiPointFirst, xPosition, yPosition));
        pointFirst.name = "Point" + id;
        ChangePosition(id, xPosition, yPosition);
    }

    public int RemoveRedPoint(int id)
    {
        Destroy(RedPoints[id].UIObj);
        Destroy(RedPoints[id].Obj);
        RedPoints.Remove(id);
        foreach (var item in RedPoints)
        {
            if (item.Key > id)
            {
                var currentPosition = item.Value.UIObj.GetComponent<Transform>().localPosition;
                var newPosition = new Vector3(currentPosition.x, currentPosition.y + 30, 0);
                item.Value.UIObj.GetComponent<Transform>().localPosition = newPosition;
            }
        }

        return 0;
    }

    private void SyncBluePointsPositions()
    {
        Point[] points = GetRedPointsAsArray();

        for (int i = 0; i < BluePoints.Count; i++)
        {
            float xPosition = BluePoints[i].XPosition;
            float yPosition = NewtonInterpolateScript.GetYByDiffTable(xPosition, points);
            BluePoints[i].ChangePosition(xPosition, yPosition);
        }
    }

    public void ChangePosition(int id, float newXposition, float newYPosition, bool sourceOfChangeIsUI = false)
    {
        RedPoints[id].ChangePosition(newXposition, newYPosition, sourceOfChangeIsUI);

        Point[] points = GetRedPointsAsArray();
        BackgroundScript.FunctionInputField.text = NewtonInterpolateScript.GenerateFunctionInterpolate(points);

        for (int i = 0; i < BluePoints.Count; i++)
        {
            float xPosition = BluePoints[i].XPosition;
            float yPosition = NewtonInterpolateScript.GetYByDiffTable(xPosition, points);
            BluePoints[i].ChangePosition(xPosition, yPosition);
        }
    }

    public int ChangePositionByUI(int id, string newXPosition, string newYPosition)
    {
        float finalXPosition = 0;
        float finalYPosition = 0;
        try
        {
            finalXPosition = float.Parse(newXPosition);
        }
        catch (System.Exception)
        {
            finalXPosition = 0;
        }

        try
        {
            finalYPosition = float.Parse(newYPosition);
        }
        catch (System.Exception)
        {
            finalYPosition = 0;
        }
        ChangePosition(id, finalXPosition, finalYPosition, true);

        return 0;
    }

    private int GenerateNewId()
    {
        return _topReferenceId++;
    }

    private void BuildBlueLine()
    {
        BluePoints = new Dictionary<int, Point>();
        var oldRoot = GameObject.Find("RootBluePoints");
        if (oldRoot != null) Destroy(oldRoot);
        var root = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity).GetComponent<Transform>();
        root.name = "RootBluePoints";

        float screenSize = 51;
        float interval = screenSize / COUNT_BLUE_POINTS;
        for (int i = 0; i < COUNT_BLUE_POINTS; i++)
        {
            float xPosition = interval * i - screenSize / 2;
            float yPosition = 0;
            int id = BluePoints.Count;
            var obj = Instantiate(BluePointReference, new Vector3(xPosition, yPosition, 0), Quaternion.identity, root);
            var point = new Point(id, obj, xPosition, yPosition);
            BluePoints.Add(BluePoints.Count, point);
        }
    }
    private Point[] GetRedPointsAsArray()
    {
        Point[] points = new Point[RedPoints.Count];
        int index = 0;
        foreach (var item in RedPoints)
        {
            points[index++] = item.Value;
        }
        return points;
    }

    public class Point
    {
        public int Id { get; }
        public GameObject Obj { get; set; }
        public float XPosition { get; set; }
        public float YPosition { get; set; }

        public Point(int id, GameObject obj, float xPosition, float yPosition)
        {
            Id = id;
            Obj = obj;
            XPosition = xPosition;
            YPosition = yPosition;
        }

        public void ChangePosition(float xPosition, float yPosition)
        {
            XPosition = xPosition;
            YPosition = yPosition;
            Obj.GetComponent<Transform>().position = new Vector3(xPosition, yPosition, 0);
        }
    }

    public class RedPoint : Point
    {
        public GameObject UIObj { get; set; }
        public RedPoint(int id, GameObject obj, GameObject uiObj, float xPosition, float yPosition) : base(id, obj, xPosition, yPosition)
        {
            UIObj = uiObj;
        }

        public void ChangePosition(float xPosition, float yPosition, bool sourceOfChangeIsUI = false)
        {
            XPosition = xPosition;
            YPosition = yPosition;
            if (sourceOfChangeIsUI)
                Obj.GetComponent<Transform>().position = new Vector3(xPosition, yPosition, -0.5f);
            else
            {
                var uiObj = UIObj.GetComponent<Transform>();
                uiObj.GetChild(UIPointsHandle.INDEX_X_INPUT_FIELD).GetComponent<InputField>().text = xPosition.ToString();
                uiObj.GetChild(UIPointsHandle.INDEX_Y_INPUT_FIELD).GetComponent<InputField>().text = yPosition.ToString();
            }
        }
    }
}
