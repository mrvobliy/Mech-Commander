using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFader : MonoBehaviour
{
    [SerializeField] private MeshRenderer _visual;
    [SerializeField] private ParticleSystem _sleepEffect;
    [SerializeField] private CanGiveArea _seatArea;

    private void Start()
    {
        _seatArea.OnDoSomething += OnAreaActivated;
        _seatArea.OnEnterInTrigger += OnEnterAreaTrigger;
    }
    private void OnDestroy()
    {
        _seatArea.OnDoSomething -= OnAreaActivated;
        _seatArea.OnEnterInTrigger -= OnEnterAreaTrigger;
    }

    private void OnEnterAreaTrigger()
    {
        FadeIn();
    }
    private void OnAreaActivated()
    {
        FadeOut();
    }
    private void FadeIn()
    {
        _visual.gameObject.SetActive(true);
        _sleepEffect.Play();
    }
    private void FadeOut()
    {
        _visual.gameObject.SetActive(false);
        _sleepEffect.Stop();
    }
}
