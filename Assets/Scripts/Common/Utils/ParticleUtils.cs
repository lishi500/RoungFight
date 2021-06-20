using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleUtils : Singleton<ParticleUtils>
{
    public void PlayParticleSystem(GameObject effect) {
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
        if (ps != null) {
            ps.Play();
        }
    }

    public void SetParticleSortingOrder(ParticleSystem ps, string layerName) {
        int sortingLayerID = SortingLayer.GetLayerValueFromName("Modal");
        ParticleSystemRenderer particleSystemRenderer = ps.GetComponent<ParticleSystemRenderer>();
        particleSystemRenderer.sortingOrder = sortingLayerID;
    }
}
