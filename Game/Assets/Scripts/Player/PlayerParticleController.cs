using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleController : MonoBehaviour
{
    private ParticleSystem particleSource;
    private void Awake()
    {
        particleSource = GetComponent<ParticleSystem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireParticle()
    {
       if (particleSource != null){
           particleSource.Play();
       }
    }
     public void StopParticle()
    {
       if (particleSource != null){
           particleSource.Stop();
       }
    }
}
