using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Illness : MonoBehaviour {
    private ParticleSystem ps;

	void Awake () {
		ps = GetComponent<ParticleSystem>();
	}
	
	void Update () {
        if (ps.shape.randomPositionAmount > (float)ManagesPlayer.instance.values.illness / 10) {
            ParticleSystem.ShapeModule shape = ps.shape;
            shape.randomPositionAmount -= Time.deltaTime;
        } else {
            ParticleSystem.ShapeModule shape = ps.shape;
            shape.randomPositionAmount = (float)ManagesPlayer.instance.values.illness / 10;
        }

        if (ManagesPlayer.instance.values.illness == 0 || !ManagesPlayer.instance.isAlive()) {
            particleEmission(ps, false);
        } else if (ManagesPlayer.instance.values.illness == 1) {
            particleEmission(ps, true);
            particleEmission(ps, 25);
        } else if (ManagesPlayer.instance.values.illness == 2) {
            particleEmission(ps, true);
            particleEmission(ps, 100);
        } else if (ManagesPlayer.instance.values.illness == 3) {
            particleEmission(ps, true);
            particleEmission(ps, 250);
        } else if (ManagesPlayer.instance.values.illness == 4) {
            particleEmission(ps, true);
            particleEmission(ps, 350);
        }
	}

    public void particleEmission(ParticleSystem PS, bool newVal) {
        ParticleSystem.EmissionModule em = PS.emission;
        em.enabled = newVal;
    }

    public void particleEmission(ParticleSystem PS, float rate) {
        ParticleSystem.EmissionModule em = PS.emission;
        em.rate = rate;
    }
}
