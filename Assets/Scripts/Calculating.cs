using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Calculating : MonoBehaviour
{
    [SerializeField] Text _output1; // Second number output
    [SerializeField] Text _outputOperator; // Operator output
    [SerializeField] Text _output2; // First number output

    StringBuilder _op1 = new StringBuilder(); // First number
    StringBuilder _operator = new StringBuilder(); // Operator
    StringBuilder _op2 = new StringBuilder("0"); // Second number

    double _num1;
    double _num2;

    bool _operatorPressed = false;
    bool _moveUp = false; // Checks if first operand is written
    bool _equalPressed = true;
    bool _powOfN = false;
    public void OnPressNumber(string num)
    {
        if (_moveUp) // Shifts first number to top of the output
        {
            _op1 = new StringBuilder(_op2.ToString()); 
            _output2.text = _op1.ToString();
            _op2.Clear();
            _op2.Append('0');
            _moveUp = false;
        }

        // If input is dot and number already contains it do nothing
        //if (num == "." && _op2.ToString().Contains("."))
        if (num == "," && _op2.ToString().Contains(","))
        {
            return;
        }

        // First input character
        if (_op2[0] == '0' && _op2.Length == 1)
        {
            //if (num == ".") // Append dot to 0
            if (num == ",")
            {
                _op2.Append(',');
            }
            else
            {
                _op2[0] = num.ToCharArray()[0]; // Replace 0 with input
            }
            
            _output1.text = _op2.ToString();
            return;
        }

        if (_op2.Length <= amountOfNumbers) // Output limit
        {
            _op2.Append(num);
            _output1.text = _op2.ToString();
        }
    }

    public void OnPressOperation(string operation)
    {
        if (_powOfN)
        {
            CountPowOfN();
            _powOfN = false;
        }

        if (_output2.text == "") // If no second operand, then just change operation
        {
            _operator.Clear();
        }
        else if (_operatorPressed) // If multiple operations used, call OnPressEqual() method between them
        {
            _equalPressed = false;
            OnPressEqual();
            _op2 = new StringBuilder(_output1.text);
        }

        if (_op2.ToString() == "0") // If input is operation after equal, continue counting
        {
            _op2 = new StringBuilder(_output1.text);
        }

        _operator.Append(operation);
        _outputOperator.text = operation.ToString();
        _operatorPressed = true;
        _moveUp = true;
    }

    public void CountPowOfN()
    {
        string[] arr = _op2.ToString().Split('^');

        double baseNum = Convert.ToDouble(arr[0]);
        double answer = baseNum;
        double powerNum = Convert.ToDouble(arr[1]);

        while (powerNum >= 2)
        {
            answer *= baseNum;
            --powerNum;
        }

        _output1.text = answer.ToString();
        _op2.Clear();
        _op2.Append(_output1.text);
            
    }

    public void OnPressEqual()
    {
        if (_powOfN)
        {
            CountPowOfN();
            _powOfN = false;
            
        }
        // If one operand is missing, then do nothing
        if (_op1.ToString() == "" || _op2.ToString() == "")
        {
            return;
        }

        _num1 = Convert.ToDouble(_op1.ToString());
        _num2 = Convert.ToDouble(_op2.ToString());
        double answer;

        if (_operator.ToString() == "+")
        {
            answer = _num1 + _num2;
            _output1.text = answer.ToString();
        }
        else if (_operator.ToString() == "-")
        {
            answer = _num1 - _num2;
            _output1.text = answer.ToString();
        }
        else if (_operator.ToString() == "x")
        {
            answer = _num1 * _num2;
            _output1.text = answer.ToString();
        }
        else if (_operator.ToString() == "/")
        {
            answer = _num1 / _num2;
            _output1.text = answer.ToString();
        }

        const int maxOutputLength = 10;

        if (_output1.text.Length > maxOutputLength) // Check if answer is longer than possible output
        {
            bool isFloat = false;

            for (int i = 0; i < maxOutputLength - 1; ++i)
            {
                // If output is floating point number then add everything after dot
                //if (_output1.text.ToString()[i] == '.')
                if (_output1.text.ToString()[i] == ',')
                {
                    isFloat = true;

                    StringBuilder forFloatingPoint = new StringBuilder();

                    for (int j = 0; j < maxOutputLength; ++j)
                    {
                        forFloatingPoint.Append(_output1.text.ToString()[j]);
                    }

                    _output1.text = forFloatingPoint.ToString();

                    break;
                }
            }

            if (!isFloat) // If not floating point reset calculator
            {
                OnPressClear();
            }
        }

        if (_equalPressed) // Set first operand to 0 after solution
        {
            _op2.Clear();
            _op2.Append("0");
            _operatorPressed = false;
        }

        _op1.Clear();
        _outputOperator.text = "";
        _output2.text = "";
        _operator.Clear();
        _equalPressed = true;
    }

    public void OnPressClear() // Reset calculator
    {
        _output1.text = "0";
        _outputOperator.text = "";
        _output2.text = "";

        _op1.Clear();
        _operator.Clear();
        _op2.Clear();
        _op2.Append("0");

        _operatorPressed = false;
        _moveUp = false;
        _equalPressed = true;
    }

    public void OnPressChangeSign()
    {
        if (_output1.text == "0") // If operand is 0 do nothing
        {
            return;
        }

        if (_op2.ToString() == "0") // If operation after equal continue counting
        {
            _op2 = new StringBuilder(_output1.text);
        }

        if (_op2[0] == '-') // Remove minus if already negative
        {
            _op2.Remove(0, 1);

        }
        else
        {
            _op2.Insert(0, "-");
        }

        _output1.text = _op2.ToString();
    }

    public void OnPressUnary(string unary)
    {
        if (_output1.text == "0") // If operand is 0 do nothing
        {
            return;
        }

        if (_op2.ToString() == "0") // If operation after equal continue counting
        {
            _op2 = new StringBuilder(_output1.text);
        }

        double num = Convert.ToDouble(_op2.ToString());

        if (unary == "sqrt")
        {
            num = Math.Sqrt(num);
        }
        else if (unary == "pow")
        {
            num *= num;
        }
        else if (unary == "pown")
        {
            _op2.Append("^");
            _output1.text = _op2.ToString();
            _powOfN = true;

            return;
        }
        else if (unary == "factorial")
        {
            // If number is not integer, do nothing
            if (num % 1 != 0)
            {
                return;
            }

            for (double i = num - 1; i >= 2; --i)
            {
                num *= i;
            }
        }
        else if (unary == "LogX")
        {
            num = Math.Log(num, 2);
        }

        _op2.Clear();
        _op2.Append(num);
        _output1.text = _op2.ToString();
    }

    public void OnPressPercent()
    {
        if (_operatorPressed)
        {
            // If one operand is missing then do nothing
            if (_op1.ToString() == "" || _op2.ToString() == "")
            {
                return;
            }

            double percent1 = Convert.ToDouble(_op1.ToString());
            double percent2 = Convert.ToDouble(_op2.ToString());

            _op2.Clear();
            _op2.Append((percent1 * percent2 / 100).ToString());

            if (_operator.ToString() == "x") // (A * B%) is (B% of a)
            {
                _output1.text = _op2.ToString();

                _op1.Clear();
                _outputOperator.text = "";
                _output2.text = "";
                _operator.Clear();
                _equalPressed = true;
            }
            else
            {
                OnPressEqual();
            }
        }
    }

    public void OnPressBackspace()
    {
        if (_op2[_op2.Length - 1] == '^')
        {
            _powOfN = false;
        }

        if (_op2.Length == 1)
        {
            _op2[0] = '0';
        }
        else
        {
            _op2.Remove(_op2.Length - 1, 1);
        }

        _output1.text = _op2.ToString();
    }

    Button _button;
    const int amountOfNumbers = 9;
    private void Update()
    {
        
        for (int i = 0; i <= amountOfNumbers; ++i)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                _button = GameObject.Find(i.ToString()).GetComponent<Button>();
                
                _button.onClick.Invoke();
                _button.Select();
            }
        }
            
        if (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.KeypadPeriod))
        {
            _button = GameObject.Find("dot").GetComponent<Button>();

            _button.onClick.Invoke();
            _button.Select();
        }
        
        else if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            _button = GameObject.Find("+").GetComponent<Button>();

            _button.onClick.Invoke();
            _button.Select();
        }
        
        else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            _button = GameObject.Find("-").GetComponent<Button>();

            _button.onClick.Invoke();
            _button.Select();
        }
        
        else if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            _button = GameObject.Find("x").GetComponent<Button>();

            _button.onClick.Invoke();
            _button.Select();
        }
        
        else if (Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.KeypadDivide))
        {
            _button = GameObject.Find(" /").GetComponent<Button>();

            _button.onClick.Invoke();
            _button.Select();
        }
        
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            _button = GameObject.Find("=").GetComponent<Button>();

            _button.onClick.Invoke();
            _button.Select();
        }
        
        else if (Input.GetKeyDown(KeyCode.Percent))
        {
            _button = GameObject.Find("%").GetComponent<Button>();

            _button.onClick.Invoke();
            _button.Select();
        }
        
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _button = GameObject.Find("Backspace").GetComponent<Button>();

            _button.onClick.Invoke();
            _button.Select();
        }

        else if (!Input.anyKey)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    //private void Update()
    //{
    //    const int amountOfNumbers = 9;
    //
    //    for (int i = 0; i <= amountOfNumbers; ++i)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
    //        {
    //            OnPressNumber(i.ToString());
    //            return;
    //        }
    //    }
    //        
    //    if (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.KeypadPeriod))
    //    {
    //        OnPressNumber(".");
    //    }
    //
    //    else if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
    //    {
    //        OnPressOperation("+");
    //    }
    //
    //    else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
    //    {
    //        OnPressOperation("-");
    //    }
    //
    //    else if (Input.GetKeyDown(KeyCode.KeypadMultiply))
    //    {
    //        OnPressOperation("x");
    //    }
    //
    //    else if (Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.KeypadDivide))
    //    {
    //        OnPressOperation("/");
    //    }
    //
    //    else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
    //    {
    //        OnPressEqual();
    //    }
    //
    //    else if (Input.GetKeyDown(KeyCode.Percent))
    //    {
    //        OnPressPercent();
    //    }
    //
    //    else if (Input.GetKeyDown(KeyCode.Backspace))
    //    {
    //        if (_op2.Length == 1)
    //        {
    //            _op2[0] = '0';
    //        }
    //        else
    //        {
    //            _op2.Remove(_op2.Length - 1, 1);
    //        }
    //
    //        _output1.text = _op2.ToString();
    //    }
    //
    //    
    //}
}


//else if (Input.GetKeyDown(KeyCode.KeypadPlus))
//{
//    Button _button;
//
//    _button = GameObject.Find("+").GetComponent<Button>();
//
//    _button.onClick.Invoke();
//    _button.Select();
//}
//
//else if (Input.GetKeyUp(KeyCode.KeypadPlus))
//{
//    EventSystem.current.SetSelectedGameObject(null);
//}
