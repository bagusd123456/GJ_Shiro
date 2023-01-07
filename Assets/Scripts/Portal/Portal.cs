using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Portal : MonoBehaviour
{
    public enum PortalType { STANDARD, BUSWAY, BUSWAY_EXIT, POWERUP }
    public Vector3 center = Vector3.zero;

    [Space]
    public PortalType _portalType;
    PlayerMovement player;

    [HideInInspector]
    public Transform target;

    float angle;

    //Color defaultColor;
    int Power;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        angle = CalculateAngle();
        //defaultColor = GetComponent<SpriteRenderer>().color;
    }
    
    public void TriggerPortal()
    {
        switch (_portalType)
        {
            case PortalType.STANDARD:
                DescendLevel();
                break;

            case PortalType.BUSWAY:
                
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

    public void testBusway()
    {
        SceneLoad.Instance.panelBusway.SetActive(true);
    }

    public void TriggerPowerUp()
    {
        Power = Random.Range(0, 3);

        if(Power == 0)
        {
            SceneLoad.Instance.GetPower(0);
            //Get Health

            PlayerCondition.Instance.GetHealth(10);
            Debug.Log("Get Health !");
        }
        if (Power == 1)
        {
            SceneLoad.Instance.GetPower(1);
            //Get MP
            PlayerCondition.Instance.GetMp(10);
            Debug.Log("Get Mana !");
        }
        if (Power == 2)
        {
            SceneLoad.Instance.GetPower(2);
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
        SceneLoad.Instance.panelBusway.SetActive(true);
        Debug.Log("Please Select Portal");
        InputHandler.Instance._state = InputHandler.state.CHOOSE;
        Time.timeScale = 0f;

        yield return new WaitUntil(() => InputHandler.Instance.selectedPortal != null);
        InputHandler.Instance._state = InputHandler.state.IDLE;
        target = InputHandler.Instance.GetSelectedPortal();
        Debug.Log("Selected");
        InputHandler.Instance.selectedPortal = null;
        
        
        Time.timeScale = 1f;
        DescendToPosition(target.localPosition);
        player.currentAngle = target.GetComponent<Portal>().angle;
        target = null;
        SceneLoad.Instance.panelBusway.SetActive(false);

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

    public void SelectThis()
    {
        InputHandler.Instance.selectedPortal = this.transform;
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
    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_portalType == PortalType.BUSWAY_EXIT)
        {
            SelectThis();
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
    */
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