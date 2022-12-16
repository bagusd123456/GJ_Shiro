using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Portal : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    
    public enum PortalType { STANDARD, BUSWAY, BUSWAY_EXIT, POWERUP }
    public Vector3 center = Vector3.zero;

    [Space]
    public PortalType _portalType;
    PlayerMovement player;

    [HideInInspector]
    public Transform target;

    float angle;

    Color defaultColor;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        angle = CalculateAngle();
        defaultColor = GetComponent<SpriteRenderer>().color;
    }
    
    public void TriggerPortal()
    {
        switch (_portalType)
        {
            case PortalType.STANDARD:
                DescendLevel();
                break;

            case PortalType.BUSWAY:
                //SceneLoad.Instance.panelBusway.SetActive(true);

                StartCoroutine(ChoosePortal());
                break;

            case PortalType.POWERUP:
                DescendLevel();
                TriggerPowerUp();
                break;

            default:
                break;
        }
    }

    public void TriggerPowerUp()
    {
        SceneLoad.Instance.GetPower();
        int Power = Random.Range(0, 2);

        if(Power == 0)
        {
            //Speed Up
            Debug.Log("Speed Up !");
        }
        if (Power == 1)
        {
            //Healt Up
            Debug.Log("Health Up !");
        }
        if (Power == 2)
        {
            //Attack Up
            Debug.Log("Attack Up !");
        }



    }
    
    public void DescendLevel()
    {
        //Get to Lower Ground
        Vector3 lastPos = player.transform.localPosition;
        player.currentLevelIndex++;
        player.transform.DOLocalMove(new Vector3(lastPos.x, 0, lastPos.z), 1.5f);
    }

    IEnumerator ChoosePortal()
    {
        Debug.Log("Please Select Portal");
        InputHandler.Instance._state = InputHandler.state.CHOOSE;
        Time.timeScale = 0f;

        yield return new WaitUntil(() => InputHandler.Instance.selectedPortal != null);
        InputHandler.Instance._state = InputHandler.state.IDLE;
        target = InputHandler.Instance.GetSelectedPortal();
        Debug.Log("Selected");

        Time.timeScale = 1f;
        DescendToPosition(target.localPosition);
        player.currentAngle = target.GetComponent<Portal>().angle;
    }

    public void DescendToPosition(Vector3 target)
    {
        //Get to Lower Ground
        player.currentLevelIndex++;
        player.transform.DOLocalMove(new Vector3(target.x, 0, target.z), 1.5f);
    }
    
    public float CalculateAngle()
    {
        Vector3 dir = center - transform.position;
        float result = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 180f;

        return result;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player") && _portalType != PortalType.BUSWAY_EXIT)
        {
            collision.GetComponent<PlayerMovement>().canGo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && _portalType != PortalType.BUSWAY_EXIT)
        {
            collision.GetComponent<PlayerMovement>().canGo = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_portalType == PortalType.BUSWAY_EXIT)
        {
            InputHandler.Instance.selectedPortal = eventData.pointerClick.transform;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_portalType == PortalType.BUSWAY_EXIT)
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_portalType == PortalType.BUSWAY_EXIT)
            gameObject.GetComponent<SpriteRenderer>().color = defaultColor;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Portal), true)]
[CanEditMultipleObjects]
public class PortalEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = target as Portal;

        base.OnInspectorGUI();

        if (script._portalType == Portal.PortalType.BUSWAY)
        {
            script.target = (Transform)EditorGUILayout.ObjectField("Target", script.target, typeof(Transform), true);
        }
    }
}
#endif