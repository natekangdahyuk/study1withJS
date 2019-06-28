using UnityEngine;
using UnityEngine.UI;


public class ChangeString : MonoBehaviour
{
    public int index;
    private void Awake()
    {
        Text text = GetComponent<Text>();
        text.text = StringTBL.GetData( index );
    }
}