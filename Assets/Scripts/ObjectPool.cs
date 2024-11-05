using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefabObject;
    [SerializeField] private int objectNumberOnStart;

    private List<GameObject> objectsPool = new List<GameObject>();

    private void Start()
    {
        
        CreateObjects(objectNumberOnStart);

    }


    /// <summary>
    /// //Create the objects needed at the begining of the game
    /// </summary>
    /// <param name="numberOfObjects"></param>
    private void CreateObjects(int numberOfObjects)
    {
        for (int i = 0; i < objectNumberOnStart; i++)
        {
            CreateNewObject();

        }
    }
    /// <summary>
    /// Instanciate new object and add to the List
    /// </summary>
    /// <returns>GameObject</returns>
    private GameObject CreateNewObject()
    {
        //Instantiate anywhere
        GameObject newObject = Instantiate(prefabObject);
        //Deactive
        newObject.SetActive(true);
        //Add to the list
        objectsPool.Add( newObject );

        return newObject;
    }

    /// <summary>
    /// Take from the List an avaiable object
    /// if not exist create a new one
    /// and Active the object
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject()
    {
        //Find in the objectsPool an object that is inactive in the game hierachy
        GameObject theObject = objectsPool.Find(x => x.activeInHierarchy == false);
        
        //if not exist, create one
        if (theObject == null)
        {
            theObject = CreateNewObject();
        }

        //Active gameobject
        theObject.SetActive(true);

        return theObject;
    }
}
