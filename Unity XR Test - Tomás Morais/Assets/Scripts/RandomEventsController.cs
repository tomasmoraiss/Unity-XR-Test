using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;

public class RandomEventsController : MonoBehaviour
{
    private enum WeatherEventType
    {
        Snow = 0,
        Rain = 1
    }

    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem snowParticleSystem;
    [SerializeField] private ParticleSystem rainParticleSystem;
    
    [Header("Game Objects")]
    [SerializeField] private GameObject waterBlockObject;
    [SerializeField] private GameObject snowBlockObject;
    
    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;
    
    [Header("Event Timers")]
    [SerializeField] private float minTimeBetweenEvents;
    [SerializeField] private float maxTimeBetweenEvents;
    [SerializeField] private float eventDuration;
    
    [Header("Interaction Text")]
    [SerializeField] private GameObject rainText;
    [SerializeField] private GameObject snowText;

    private const int EVENT_TYPE_COUNT = 2;

    private void Start()
    {
        StartCoroutine(EventLoop());
        ClearEvents();
    }
    
    private IEnumerator EventLoop()
    {
        while (true)
        {
            //Choose a random time from the min and the max time between the events to add randomness to the events
            float timeBetweenEvents = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
            yield return new WaitForSeconds(timeBetweenEvents);

            if (CanTriggerNewEvent())
            {
                TriggerRandomWeatherEvent();
                yield return new WaitForSeconds(eventDuration);
            }
            ClearEvents();
        }
    }

    private bool CanTriggerNewEvent()
    {
        return !gameManager.isSnowing && !gameManager.isRaining && 
               !snowBlockObject.activeSelf && !waterBlockObject.activeSelf;
    }

    private void TriggerRandomWeatherEvent()
    {
        WeatherEventType eventType = (WeatherEventType)Random.Range(0, EVENT_TYPE_COUNT);
        switch (eventType)
        {
            case WeatherEventType.Snow when snowParticleSystem:
                TriggerSnowEvent();
                snowBlockObject.SetActive(true);
                break;
            case WeatherEventType.Rain when rainParticleSystem:
                TriggerRainEvent();
                waterBlockObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    //Clear all effects from the random events
    private void ClearEvents()
    {
        StopWeatherEffects();
    }
    
    private void StopWeatherEffects()
    {
        snowParticleSystem?.Stop();
        rainParticleSystem?.Stop();
    }
    
    private void TriggerSnowEvent()
    {
        gameManager.isSnowing = true;
        gameManager.canBuild = false;
        snowText.gameObject.SetActive(true);
        snowBlockObject.SetActive(true);
        
        if (!snowParticleSystem.isPlaying)
        {
            snowParticleSystem.Play();
        }
    }

    private void TriggerRainEvent()
    {
        gameManager.isRaining = true;
        gameManager.canBuild = false;
        rainText.gameObject.SetActive(true);
        if (!rainParticleSystem.isPlaying)
        {
            rainParticleSystem.Play();
        }
    }
    
}
