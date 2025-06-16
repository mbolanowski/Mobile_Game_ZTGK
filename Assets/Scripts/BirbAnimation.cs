using System.Collections;
using UnityEngine;

public class BirbAnimation : MonoBehaviour
{

    public int minFlap;
    public int maxFlap;

    public float flapDelayMin;
    public float flapDelayMax;

    private int targetFlapCount;

    private int flapCount = 0;

    public Animator animator;

    public void WingFlap()
    {
        if(flapCount == 0)
        {
            targetFlapCount = Random.Range(minFlap, maxFlap);
        }
        flapCount++;
        if(flapCount == targetFlapCount)
        {
            animator.speed = 0;
            StartCoroutine("ResumeFlap");
            flapCount = 0;
        }

    }

    IEnumerator ResumeFlap()
    {
        float flapDelay = Random.Range(flapDelayMin, flapDelayMax);
        yield return new WaitForSeconds(flapDelay);
        animator.speed = 1;
    }

}
