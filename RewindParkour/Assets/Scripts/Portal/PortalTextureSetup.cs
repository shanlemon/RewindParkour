using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Setups the material and textures for the portals. 
/// </summary>
public class PortalTextureSetup : MonoBehaviour
{
    [SerializeField] private Material cameraMat = default;
    [SerializeField] private Camera camera = default;

    // Start is called before the first frame update
    void Start()
    {
        if (camera.targetTexture != null)
        {
            camera.targetTexture.Release();
        }

        camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMat.mainTexture = camera.targetTexture;
        
    }

}
