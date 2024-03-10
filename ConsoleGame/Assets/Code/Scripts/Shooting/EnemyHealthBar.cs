using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    public Image foregroundImg;
    public Image backgroundImg;
    public Vector3 offsetBar;
    public float maxViewDistance = 100;


    void LateUpdate()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(target.position + offsetBar);

        // Check if the target is within the camera's view range
        float distanceToTarget = Vector3.Distance(Camera.main.transform.position, target.position);
        bool isTargetVisible = screenPoint.z > 0 && distanceToTarget <= maxViewDistance;

        foregroundImg.enabled = isTargetVisible;
        backgroundImg.enabled = isTargetVisible;

        if (isTargetVisible)
        {
            transform.position = screenPoint;
        }
    }

    public void HealthBarPercentage(float percentage)
    {
        float barWidth = GetComponent<RectTransform>().rect.width;
        float width = barWidth * percentage;
        foregroundImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
