using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour
{


    [SerializeField] private ObjectPoolBehaviour _fXPool;
    [SerializeField] private ParticleSystemForceField _forceField;

    private void OnEnable()
    {
        GameDelegates.OnPlayPooledFX += PlayPooledFX;
    }
    private void OnDisable()
    {
        GameDelegates.OnPlayPooledFX -= PlayPooledFX;
    }

    private void PlayPooledFX(Vector3 screenPos)
    {
        Vector3 worldPos = screenPos;

        // initialize ParticleSystem
        ParticleSystem ps = _fXPool.GetPooledObject().GetComponent<ParticleSystem>();

        if (ps == null)
            return;

        ps.gameObject.SetActive(true);
        ps.gameObject.transform.position = worldPos;
        ParticleSystem.ExternalForcesModule externalForces = ps.externalForces;
        externalForces.enabled = true;

        // add the Forcefield for destination
        _forceField.gameObject.SetActive(true);
        externalForces.AddInfluence(_forceField);

        ps.Play();
    }
}