using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{

    [SerializeField] GameObject exp;
    [SerializeField] float expForce, radius;
    private BombParentScript bombParentScript;

    private void Start() {
        bombParentScript = transform.parent.gameObject.GetComponent<BombParentScript>();
    }

    private void Update() {
        if(bombParentScript.GetExplode())
        {
            StartCoroutine(WaitAndDo(Knockback, 1f));
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "man")
        {

        Knockback();
        
        //gameObject.SetActive(false);
        bombParentScript.SetExplode(true);
        Destroy(gameObject);
        }
    }

    void Knockback()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider nearby in colliders)
        {
            Rigidbody rigg = nearby.GetComponent<Rigidbody>();

            if(rigg != null)
            {
                rigg.AddExplosionForce(expForce, transform.position, radius);
            }
        }
        GameObject _exp = Instantiate(exp, transform.position, transform.rotation);
        Destroy(_exp, 3);
        
    }

    public IEnumerator WaitAndDo(System.Action action, float waitingTime)
    {
        float time = 0;
        
        while(time < waitingTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        action?.Invoke();

        //extension
        Destroy(gameObject);

    }
}
