
using TMPro;
using UnityEngine;

public class HordeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _stackCountText;

    private void Awake()
    {
        HordeManager.OnHordeCountChange += ChangeStackText;
    }

    private void OnDestroy()
    {
        HordeManager.OnHordeCountChange -= ChangeStackText;
    }

    private void ChangeStackText(int count)
    {
        _stackCountText.text = count.ToString();
    }
}
