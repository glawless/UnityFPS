using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open;

    public void Open()
    {
        open = true;
        if (this != null)
            gameObject.SetActive(true);
    }

    public void Close()
    {
        open = false;
        if (this != null)
            gameObject.SetActive(false);
    }
}
