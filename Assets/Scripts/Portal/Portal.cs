using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Portal : MonoBehaviour, IPointerClickHandler
{
    public bool clicked;

    public enum PortalType { STANDARD, BUSWAY, BUSWAY_EXIT, POWERUP }
    public PortalType _portal;
    PlayerMovement player;

    [HideInInspector]
    public Transform target;

    public float angle;
    public Vector3 center = Vector3.zero;
    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        angle = CalculateAngle();
    }
    
    public void TriggerPortal()
    {
        switch (_portal)
        {
            case PortalType.STANDARD:
                DescendLevel();
                break;

            case PortalType.BUSWAY:
                //SceneLoad.Instance.panelBusway.SetActive(true);

                StartCoroutine(ChoosePortal());
                break;

            case PortalType.BUSWAY_EXIT:
                break;

            case PortalType.POWERUP:
                break;

            default:
                break;
        }
    }

    IEnumerator ChoosePortal()
    {
        Debug.Log("Please Select Portal");
        InputHandler.Instance._state = InputHandler.state.CHOOSE;
        Time.timeScale = 0f;

        yield return new WaitUntil(()=> InputHandler.Instance.selectedPortal != null);

        target = InputHandler.Instance.GetSelectedPortal();
        Debug.Log("Selected");

        Time.timeScale = 1f;
        DescendToPosition(target.localPosition);
        player.currentAngle = target.GetComponent<Portal>().angle;
    }

    public void DescendLevel()
    {
        //Get to Lower Ground
        Vector3 lastPos = player.transform.localPosition;
        player.currentLevelIndex++;
        player.transform.DOLocalMove(new Vector3(lastPos.x, 0, lastPos.z), 1.5f);
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
        if(collision.tag.Equals("Player"))
        {
            
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerClick.transform);
        
        if (_portal == PortalType.BUSWAY_EXIT)
        {
            InputHandler.Instance.selectedPortal = eventData.pointerClick.transform;
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(Portal), true)]
public class PortalEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = target as Portal;

        base.OnInspectorGUI();

        if (script._portal == Portal.PortalType.BUSWAY)
        {
            script.target = (Transform)EditorGUILayout.ObjectField("Target", script.target, typeof(Transform), true);
        }
    }
}
#endif