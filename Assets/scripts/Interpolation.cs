using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interpolation : MonoBehaviour
{
    const int sizeRed = 300;
    public string function;
    public string functionPosfix;
    public GameObject p;
    public GameObject red;
    //public int size;
    public List<GameObject> points;
    public GameObject[] pointRed;
    public List<double> d;
    public List<double> previous;
    public List<double> current;
    public HandlePoints handlePoints;
    public Toggle realTime;
    public float fps;
    public float sizeRay;
    public float sizeCan;

    // Start is called before the first frame update
    void Start()
    {
        load();
    }

    // Update is called once per frame
    void Update()
    {
        fps = 1.0f / Time.deltaTime;
        if (realTime.isOn)
        {
            interpolate();
        }
        
    }

    public void interpolate()
    {
        for (int i = 0; i < points.Count; i++)
        {
            previous[i] = points[i].transform.position.y;
        }
        d[0] = points[0].transform.position.y;
        for (int i = 0; i < points.Count - 1; i++)
        {
            for (int j = 0; j < points.Count - i - 1; j++)
            {
                current[j] = (previous[j + 1] - previous[j]) / (points[i + j + 1].transform.position.x - points[j].transform.position.x);
            }
            d[i + 1] = current[0];
            for (int k = 0; k < points.Count - i - 1; k++)
            {
                previous[k] = current[k];
            }
        }
        function = "";
        for (int i = 0; i < points.Count; i++)
        {
            function += "(" + d[i].ToString("N6") + ")";
            for (int j = 0; j < i; j++)
            {
                function += "*(x - (" + points[j].transform.position.x.ToString("N6") + "))";
            }
            if (i < points.Count - 1)
            {
                function += " + ";
            }
        }
        functionPosfix = posFixa(function);
        function = function.Replace(',', '.');
        handlePoints.func.text = "f(x) = " + function;

        float angle = Vector2.Angle(transform.right, points[1].transform.position);
        //float coeficient = Mathf.Pow(Mathf.Sin(angle * Mathf.PI / 180), 2);
        sizeRay = 16.5f/ sizeCan;
        for (int i = 0; i < sizeRed; i++)
        {
            float x = i/sizeRay - 9*sizeCan;
            float fx = f(x);
            pointRed[i].transform.position = new Vector2(x, fx);
        }

        handlePoints.updateUIPoints(points);
    }
    public void handleSizePoints(float size)
    {
        for (int i = 0; i < points.Count; i++)
        {
            points[i].transform.localScale = Vector3.one * size;
        }
        for (int i = 0; i < sizeRed; i++)
        {
            pointRed[i].transform.localScale = Vector3.one * size;
        }
    }
    public void load()
    {
        int sizeInit = 3;
        points = new List<GameObject>();
        d = new List<double>();
        previous = new List<double>();
        current = new List<double>();
        pointRed = new GameObject[sizeRed];
        for (int i = 0; i < sizeInit; i++)
        {
            addPoint();
        }

        for (int i = 0; i < sizeRed; i++)
        {
            pointRed[i] = Instantiate(red, Vector3.zero, transform.rotation);
        }

        interpolate();
    }
    public void addPoint()
    {
        Vector2 pos = new Vector2(points.Count / 5.0f, points.Count / 5.0f);
        handlePoints.addPoint();
        points.Add(Instantiate(p, pos, transform.rotation));
        d.Add(0);
        previous.Add(0);
        current.Add(0);
        PointUI.verifyCanRemove();
    }
    public void removePoint(int index)
    {
        Destroy(handlePoints.pointsUI[index]);
        handlePoints.pointsUI.RemoveAt(index);
        Destroy(points[index]);
        points.RemoveAt(index);
        d.RemoveAt(index);
        previous.RemoveAt(index);
        current.RemoveAt(index);
    }
    float f(float x)
    {
        float result = 0;
        float []op = { 0, 0 };
        string c = functionPosfix;
        bool findOpt = false;
        int i = 0;
        for (int u = 0; u < c.Length; u++)
        {
            if (c[u] == '+' || c[u] == '-' || c[u] == '*' || c[u] == '/')
            {
                findOpt = true;
            }
        }
        if (!findOpt && (c[i] >= '0' && c[i] <= '9') || c[i] == '.' || c[i] == ',' || c[i] == 'x')
        {
            if (c[i] == 'x')
            {
                op[0] = x;
            }
            else
            {
                op[0] = float.Parse(c);
            }
        }
        while (findOpt)
        {
            if (c[i] == '+' || c[i] == '-' || c[i] == '*' || c[i] == '/') //search operation
            {
                char operation = c[i];
                int k = i - 1;
                int j;
                string auxOp = "";
                for (j = 0; j < 2; j++) //search two numbers
                {
                    if (c[k] != 'x')
                    {
                        bool find = false;
                        do
                        {
                            if ((c[k] >= '0' && c[k] <= '9') || c[k] == '.' || c[k] == ',')
                            {
                                auxOp = c[k] + auxOp;
                                find = true;
                            }
                            k--;
                        } while (k >= 0 && c[k] != 'x' && c[k] != ' ');
                        if (find)
                        {
                            op[j] = float.Parse(auxOp);
                            if (c[k + 1] == 'N')
                            {
                                op[j] *= -1;
                            }
                        }
                        else
                        {
                            if(c[k] == 'x')
                            {
                                k--;
                                op[j] = x;
                            }
                            else
                            {
                                k = -1;
                                j--;
                            }
                        }
                    }
                    else
                    {
                        k--;
                        op[j] = x;
                    }
                    auxOp = "";
                }

                switch (operation)
                {
                    case '+':
                        op[0] = op[1] + op[0];
                        break;
                    case '-':
                        op[0] = op[1] - op[0];
                        break;
                    case '*':
                        op[0] = op[1] * op[0];
                        break;
                    case '/':
                        op[0] = op[1] / op[0];
                        break;
                    default:
                        op[0] = op[1] + op[0];
                        break;
                }

                string rest = c.Substring(i + 1, c.Length - (i + 1));
                if (i + 1 >= c.Length)
                {
                    break;
                }
                else
                {
                    i = 0;
                }

                c = c.Substring(0, k + 1);
                string opstr;
                if (op[0] >= 0)
                {
                    opstr = op[0].ToString("N6");
                    if (opstr[0] == '-')
                    {
                        opstr = opstr.Substring(1, opstr.Length - 1);
                    }
                }
                else
                {
                    opstr = "N" + (-op[0]).ToString("N6");
                }
                c = c + opstr + " " + rest;
                op[1] = 0;
            }
            i++;
        }
        result = op[0];
        return result;
    }

    string posFixa(string str)
    {
        for (int p = 0; p < str.Length - 1; p++)
        {
            if (str[p] == '(' && (str[p + 1] == '+' || str[p + 1] == '-'))
            {
                string rest = str.Substring(p + 1, (str.Length - (p + 1)));
                str = str.Substring(0, p + 1) + "0" + rest;
            }
        }
        int i = 0;
        char c =' ', t;
        Stack<char> stack = new Stack<char>();
        string result = "";
        stack.Push('(');
        do
        {
            if (i < str.Length)
                c = str[i];
            else
                c = '\0';
            if ((c >= '0' && c <= '9') || c == 'x' || c == '.' || c == ',')
            {
                string number = "";
                if (c != 'x')
                {
                    for (int j = 0; j < 50; j++) //50 is the most size to find a number
                    {
                        if ((str[i] >= '0' && str[i] <= '9') || str[i] == '.' || str[i] == ',')
                        {
                            number += str[i];
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (number != "")
                {
                    string aux = float.Parse(number).ToString("N6");
                    result += aux + " ";
                    i--;
                }
                else
                    result += c;
            }
            else if (c == '(')
            {
                stack.Push('(');
            }
            else if (c == ')' || c == '\0')
            {
                do
                {
                    t = stack.Pop();
                    if (t != '(')
                    {
                        result += t;
                    }
                } while (t != '(');
            }
            else if (c == '+' || c == '-' || c == '*' || c == '/')
            {
                while (true)
                {
                    t = stack.Pop();
                    if (priority(c, t))
                    {
                        stack.Push(t);
                        stack.Push(c);
                        break;
                    }
                    else
                    {
                        result += t;
                    }
                }
            }
            i++;
        } while (c != '\0');

        return result;
    }
    bool priority(char c, char t)
    {
        int pc =0, pt = 0;

        if (c == '*' || c == '/')
            pc = 2;
        else if (c == '+' || c == '-')
            pc = 1;
        else if (c == '(')
            pc = 4;

        if (t == '*' || t == '/')
            pt = 2;
        else if (t == '+' || t == '-')
            pt = 1;
        else if (t == '(')
            pt = 0;

        if (pc > pt)
            return true;
        else
            return false;
    }
}
