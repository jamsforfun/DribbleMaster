using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

/// <summary>
/// Display fps in game
/// </summary>
public class FPSDisplay : MonoBehaviour
{
    [FoldoutGroup("GamePlay"), Tooltip("Number to fill"), SerializeField]
    private Image[] _imagesCanvas = default;
    // in order, like 0, 1, 2, ..., 9
    [FoldoutGroup("GamePlay"), Tooltip("All sprites"), SerializeField]
    private Sprite[] _numberSprites = default;

    FrequencyCoolDown FrequencyCoolDown = new FrequencyCoolDown();

    private const float TimeBetweenChange = 0.01f;

    private float _deltaTime = 0.0f;
    private float _fps;
    private float _mSec;
    private float _previousFps = float.MinValue;

    private void OnEnable()
    {
        FrequencyCoolDown.StartCoolDown(TimeBetweenChange);
    }

    public void SetNumber(int number, Image imageTens, Image imageOnes)
    {
        int tens = (number % 100) / 10;
        int ones = (number % 10);

        imageTens.sprite = _numberSprites[tens];
        imageOnes.sprite = _numberSprites[ones];
    }

    private void Update()
    {
        if (FrequencyCoolDown.IsReady())
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;

            _mSec = _deltaTime * 1000.0f;
            _fps = 1.0f / _deltaTime;

            if (_fps != _previousFps)
            {
                SetNumber((int)_mSec, _imagesCanvas[0], _imagesCanvas[1]);
                SetNumber((int)_fps, _imagesCanvas[2], _imagesCanvas[3]);
                _previousFps = _fps;
            }
            
            FrequencyCoolDown.StartCoolDown(TimeBetweenChange);
        }
    }
}