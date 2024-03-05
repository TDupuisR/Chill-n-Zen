using GameManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackSignManager : MonoBehaviour
{
    [SerializeField] private GameObject _displayMoney;
    public float facteurPulsation = 1.2f;
    private float _size;
    private Vector3 _initialSize;
    private bool _pulse = false;

    private void OnEnable()
    {
        TileSystem.OnItemAdded += StartPulse;
    }

    private void OnDisable()
    {
        TileSystem.OnItemAdded -= EndPulse;
    }

    private void StartPulse(Item item)
    {
        if(GameManager.budgetManager.IsOutOfBudget() == true)
        {
            _pulse = true;
        }
    }

    private void EndPulse(Item item)
    {
        _pulse = false;
    }

    IEnumerator PulsationCoroutine()
    {
        _pulse = true;

        float tempsDebut = Time.time;
        float tempsEcoule = 0f;

        while (_pulse)
        {
            float echelle = Mathf.Lerp(1f, facteurPulsation, _size);

            _displayMoney.transform.localScale = _initialSize * echelle;

            tempsEcoule = Time.time - tempsDebut;

            yield return null;
        }
        _displayMoney.transform.localScale = _initialSize;
    }
}
