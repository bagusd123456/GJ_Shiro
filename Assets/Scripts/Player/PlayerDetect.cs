using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("BatasKanan"))
        {
            RotateObject.Instance.batasKanan = true;
        }
        if (collision.tag.Equals("BatasKiri"))
        {
            RotateObject.Instance.batasKiri = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("BatasKanan"))
        {
            RotateObject.Instance.batasKanan = false;
        }
        if (collision.tag.Equals("BatasKiri"))
        {
            RotateObject.Instance.batasKiri = false;
        }
    }
}
