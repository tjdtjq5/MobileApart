using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    [SerializeField] Transform slashBlue01Transform;

    int slashBlue01Count;
    public void SlashBlue01()
    {
        if (slashBlue01Count == slashBlue01Transform.childCount)
        {
            slashBlue01Count = 0;
        }
        GameObject hitParticle = slashBlue01Transform.GetChild(slashBlue01Count).gameObject;
        float randomPosX = Random.Range(-75, 75);
        float randomPosY = Random.Range(-75, 75);
        hitParticle.transform.localPosition = new Vector2(randomPosX, randomPosY);
        hitParticle.GetComponent<ParticleSystem>().Play();

        slashBlue01Count++;
    }
}
