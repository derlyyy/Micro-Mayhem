using System;
using Sonity;
using UnityEngine;

public class Box : MonoBehaviour
{
    public SoundEvent soundImpact;

    private void OnCollisionEnter2D(Collision2D other)
    {
        SoundManager.Instance.Play(soundImpact, transform);
    }
}