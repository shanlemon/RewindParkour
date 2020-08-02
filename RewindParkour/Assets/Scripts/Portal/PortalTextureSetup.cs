using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Setups the material and textures for the portals. Currently on the 
/// GameManager but should probably be moved onto one of the portals in a pair.
/// </summary>
public class PortalTextureSetup : MonoBehaviour
{
    [SerializeField] private Material cameraMatA = default;
    [SerializeField] private Camera cameraA = default;
    
    [SerializeField] private Material cameraMatB = default;
    [SerializeField] private Camera cameraB = default;

    // Start is called before the first frame update
    void Start()
    {
        if (cameraA.targetTexture != null)
        {
            cameraA.targetTexture.Release();
        }

        cameraA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatA.mainTexture = cameraA.targetTexture;

        if (cameraB.targetTexture != null)
        {
            cameraB.targetTexture.Release();
        }

        cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatB.mainTexture = cameraB.targetTexture;
        
    }

}
