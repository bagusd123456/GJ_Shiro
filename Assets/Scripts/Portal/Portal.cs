using DG.Tweening;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Portal : MonoBehaviour
{
    public enum PortalType { STANDARD, BUSWAY, BUSWAY_EXIT, POWERUP }
    public Vector3 center = Vector3.zero;
    public GameObject initEffect;

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
        if(initEffect == null)
            initEffect = Resources.Load("Teleport/PortalVFX") as GameObject;

        player = FindObjectOfType<PlayerMovement>();
        angle = CalculateAngle();
    }

    private void Start()
    {
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
        player.currentLevelIndex++;
        var go = Instantiate(initEffect, transform.position,Quaternion.identity,transform);
        player.targetPos = transform.localPosition;
        //go.transform.localPosition = Vector3.zero;
    }

    IEnumerator ChoosePortal()
    {
        SceneLoad.Instance.panelBusway.SetActive(true);
        SceneLoad.Instance.movBtn.SetActive(false);
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
        SceneLoad.Instance.movBtn.SetActive(true);

    }

    public void DescendToPosition(Vector3 target)
    {
        //Get to Lower Ground
        player.currentLevelIndex++;
        var go = Instantiate(initEffect, transform.position, Quaternion.identity, transform);
        player.targetPos = target;
        //player.transform.DOLocalMove(new Vector3(target.x, 0, target.z), 1.5f);
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

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag.Equals("Player") && _portalType != PortalType.BUSWAY_EXIT)
        {
            collision.GetComponent<PlayerMovement>().canGo = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag.Equals("Player") && _portalType != PortalType.BUSWAY_EXIT)
        {
            collision.GetComponent<PlayerMovement>().canGo = false;
        }
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