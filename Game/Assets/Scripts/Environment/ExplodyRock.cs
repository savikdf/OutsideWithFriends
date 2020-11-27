using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ExplodyRock : MonoBehaviour
{
    List<Rigidbody> rocks;

    private void Awake()
    {
        rocks = GetComponentsInChildren<Rigidbody>().ToList();
    }

    async void BoomAsyn() {
        this.GetComponent<AudioSource>().Play();
        await Task.Delay(300);
        rocks.ForEach(r => {
            r.isKinematic = false;
            r.AddExplosionForce(5f, r.position, 0f, 1f, ForceMode.Impulse);
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            BoomAsyn();
            this.GetComponent<Collider>().enabled = false;
        }
    }


}
