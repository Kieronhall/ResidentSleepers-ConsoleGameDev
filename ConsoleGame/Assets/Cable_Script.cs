using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable_Script : MonoBehaviour
{
    public RectTransform imageRectTransform;
    public Camera targetCamera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckPixelColorUnderImageCorners();
        }
    }

    private void CheckPixelColorUnderImageCorners()
    {
        Vector3[] corners = new Vector3[4];
        imageRectTransform.GetWorldCorners(corners);

        foreach (Vector3 corner in corners)
        {
            // Convert world position of corner to screen position
            Vector3 screenPos = targetCamera.WorldToScreenPoint(corner);

            // Cast a ray from the corner onto the scene
            Ray ray = targetCamera.ScreenPointToRay(screenPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Get the color of the pixel underneath the raycast hit point
                Texture2D texture = hit.collider.gameObject.GetComponent<Renderer>().material.mainTexture as Texture2D;

                if (texture != null)
                {
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.x *= texture.width;
                    pixelUV.y *= texture.height;

                    Color color = texture.GetPixel((int)pixelUV.x, (int)pixelUV.y);
                    Debug.Log("Color under corner: " + color);
                }
                else
                {
                    Debug.LogError("No texture found on the object underneath the corner.");
                }
            }
            else
            {
                Debug.LogError("No object found underneath the corner.");
            }
        }
    }
}
