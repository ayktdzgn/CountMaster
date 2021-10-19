using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum OperatorType
{
    Add,
    Sub,
    Mul,
    Div
}

public class Gate : MonoBehaviour
{
    [SerializeField] Collider _collider;
    [SerializeField] private int _operatorValue;
    [SerializeField] OperatorType _operatorType;
    [SerializeField] TMP_Text _operatorText;

    [SerializeField] Color blueColor;
    [SerializeField] Color redColor;
    [SerializeField] Image panelImage;

    GateManager _gateManager;

    public Collider Collider => _collider;

    private void Awake()
    {
        _gateManager = GetComponentInParent<GateManager>();
        SetOperatorUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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
                panelImage.color = blueColor;
                break;
            case OperatorType.Mul:
                _operatorText.text = "x" + _operatorValue.ToString();
                panelImage.color = blueColor;
                break;
            case OperatorType.Sub:
                _operatorText.text = "-" + _operatorValue.ToString();
                panelImage.color = redColor;
                break;
            case OperatorType.Div:
                _operatorText.text = "÷" + _operatorValue.ToString();
                panelImage.color = redColor;
                break;
        }
    }

}
