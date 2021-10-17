using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum OperatorType
{
    Add,
    Sub,
    Mul,
    Div
}

public class Gate : MonoBehaviour
{
    [SerializeField] private int _operatorValue;
    [SerializeField] OperatorType _operatorType;
    [SerializeField] TMP_Text _operatorText;
    GateManager _gateManager;

    private void Awake()
    {
        _gateManager.GetComponentInParent<GateManager>();
        SetOperatorUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _gateManager.GateTriggered();
            HordeManager.OnHordeChange?.Invoke(_operatorValue, _operatorType);
        }
    }

    public void SetOperatorUI()
    {
        switch (_operatorType)
        {
            case OperatorType.Add:
                _operatorText.text = "+" + _operatorValue.ToString();
                break;
            case OperatorType.Mul:
                _operatorText.text = "x" + _operatorValue.ToString();
                break;
            case OperatorType.Sub:
                _operatorText.text = "-" + _operatorValue.ToString();
                break;
            case OperatorType.Div:
                _operatorText.text = "÷" + _operatorValue.ToString();
                break;
        }
    }

}
