using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _lifeText;

    public void UpdateLife(int lives)
    {
        _lifeText.text = string.Format("Life: {0}", lives);
    }
}