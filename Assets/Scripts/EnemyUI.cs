using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] TMP_Text _stackCountText;

    public void StackCountChange(int stackCount)
    {
        _stackCountText.text = stackCount.ToString();
    }
    public void CloseText()
    {
        this.gameObject.SetActive(false);
    }
}
