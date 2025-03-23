using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
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

    private void ClearEvents()
    {
        StopWeatherEffects();
        ResetWeatherState();
    }
    
    private void StopWeatherEffects()
    {
        snowParticleSystem?.Stop();
        rainParticleSystem?.Stop();
    }

    private void ResetWeatherState()
    {
        gameManager.isSnowing = false;
        gameManager.isRaining = false;
        snowBlockObject.SetActive(false);
        waterBlockObject.SetActive(false);
    }
    
    private void TriggerSnowEvent()
    {
        gameManager.isSnowing = true;
        snowBlockObject.SetActive(true);
        
        if (!snowParticleSystem.isPlaying)
        {
            snowParticleSystem.Play();
        }
    }

    private void TriggerRainEvent()
    {
        gameManager.isRaining = true;
        
        if (!rainParticleSystem.isPlaying)
        {
            rainParticleSystem.Play();
        }
    }
    
}
