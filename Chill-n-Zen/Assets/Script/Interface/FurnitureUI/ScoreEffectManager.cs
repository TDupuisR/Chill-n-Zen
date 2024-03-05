using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEffectManager : MonoBehaviour
{
    [SerializeField] GameObject _scoreEffectPrefab;
    [SerializeField] Transform _starSliderTransform;

    public void SpawnEffect(Vector3 startingPosition, List<string> textList)
    {
        GameObject newEffect = Instantiate(_scoreEffectPrefab, startingPosition, Quaternion.identity);
        ItemScoreEffect effectScript = newEffect.GetComponent<ItemScoreEffect>();

        effectScript.EndingPosition = _starSliderTransform.position;
        effectScript.TextList = textList;
        effectScript.StartEffect();
    }

    [Button]
    public void TestDisplayEffect()
    {
        List<string> list = new List<string>()
        {
            "+99 - fouet",
            "*2 - menottes"
        };
        SpawnEffect(Vector3.zero, list);
    }
}
