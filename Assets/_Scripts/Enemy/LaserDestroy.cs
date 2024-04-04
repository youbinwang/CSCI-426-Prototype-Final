using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroy : MonoBehaviour
{
    [SerializeField] public float destroyTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestroy()); ;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SelfDestroy()
    {

        yield return new WaitForSeconds(destroyTime);

        Destroy(this.gameObject);
    }
}
