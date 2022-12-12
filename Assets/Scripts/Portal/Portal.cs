using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public enum PortalType {Standard ,Busway , PowerUp}
    public PortalType _portal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_portal == PortalType.Busway && collision.tag.Equals("Player"))
        {
            RotateObject.Instance.buswayPortal = true;
        }
    }
}
