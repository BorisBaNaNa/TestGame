using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GroundSpawner))]
public class GroundController : MonoBehaviour
{
    public GroundController LastGround, NextGround;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && NextGround == null)
        {
            Vector3 buildPos = transform.position;
            buildPos.x += transform.localScale.x;

            NextGround = AllServices.GetService<FactoryGround>().BuildGround(buildPos);
            NextGround.LastGround = this;
            NextGround.transform.parent = transform.parent;

            GroundSpawner _spawner = NextGround.GetComponent<GroundSpawner>();
            _spawner.SpawnEnemies();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && LastGround != null)
        {
            Destroy(LastGround.gameObject);
        }
    }
}
