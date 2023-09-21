using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DualWorld : MonoBehaviour
{
    public KeyCode switchKey = KeyCode.Space;

    private bool spawnWorld1 = true;

    private GameObject[] world1Objects;
    private GameObject[] world2Objects;
    void Start()
    {
        // Get all objects with the specified tags
        
        switchWorlds();
    }
    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            spawnWorld1 = !spawnWorld1;
            switchWorlds();
        }
    }

    public string SceneNum;

    public void SetScene(string sceneNum)
    {
        SceneNum = sceneNum;

        SceneManager.LoadScene(SceneNum);
    }
    
    void switchWorlds()
    {
        
        GameObject[] world1Objects = GameObject.FindGameObjectsWithTag("World1");
        GameObject[] world2Objects = GameObject.FindGameObjectsWithTag("World2");
        
        if (spawnWorld1 == true)
        {        
           
            
            foreach (GameObject obj in world1Objects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                }

                BoxCollider2D boxCollider = obj.GetComponent<BoxCollider2D>();
                if (boxCollider != null)
                {
                    boxCollider.enabled = true;
                }
            }
            foreach (GameObject obj in world2Objects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }

                BoxCollider2D boxCollider = obj.GetComponent<BoxCollider2D>();
                if (boxCollider != null)
                {
                    boxCollider.enabled = false;
                }
            }
        }
        else
        {
            foreach (GameObject obj in world1Objects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }

                BoxCollider2D boxCollider = obj.GetComponent<BoxCollider2D>();
                if (boxCollider != null)
                {
                    boxCollider.enabled = false;
                }
            }
            foreach (GameObject obj in world2Objects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                }

                BoxCollider2D boxCollider = obj.GetComponent<BoxCollider2D>();
                if (boxCollider != null)
                {
                    boxCollider.enabled = true;
                }
            }
        }
    }
}