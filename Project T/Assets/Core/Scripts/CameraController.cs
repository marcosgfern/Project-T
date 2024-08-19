using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class CameraController is used as a component of the main camera.
 * Adjusts size an position to adapt to different screen ratios and sizes.
 * Moves camera to corresponding room.
 */
public class CameraController : MonoBehaviour 
{
    [Header("Shaking parameters")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeStrength;
    [SerializeField] private int shakeVibrato;
    [SerializeField] private float shakeRandomness;
    [SerializeField] private bool shakeFadeOut;
    [SerializeField] private ShakeRandomnessMode shakeRandomnessMode;
    [SerializeField] private float timeFreeze;

    [Header("References")]
    [SerializeField] private RectTransform upperUI;
    [SerializeField] private RectTransform lowerUI;
    [SerializeField] private RectTransform canvas;
    
    private float shift;
    private float panningDuration = 0.2f;

    void Awake()
    {
        AdjustOrtographicSize();
        CalculateCameraYShift();
    }

    private void Start()
    {
        StartCoroutine(ShakeRepeater());
    }

    public float GetPanningDuration()
    {
        return this.panningDuration;
    }

    /* Calculates and sets the Y shift of the camera, necessary to center rooms in the space between lower and upper UI panels. */
    public void CalculateCameraYShift()
    {
       float screenShift = (lowerUI.rect.height - upperUI.rect.height) / 2;
        this.shift = (Camera.main.ScreenToWorldPoint(new Vector3(0, screenShift, 0)) - Camera.main.ScreenToWorldPoint(Vector3.zero)).y;
    }

    /* Adjusts orthographic size to fix the width ratio in different screen sizes (Unity fixes height by default) */
    private void AdjustOrtographicSize()
    {
        SpriteRenderer aspectRatioModel = GetComponentInChildren<SpriteRenderer>();

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = aspectRatioModel.bounds.size.x / aspectRatioModel.bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = aspectRatioModel.bounds.size.y / 2;
        } 
        else
        {
            Camera.main.orthographicSize = aspectRatioModel.bounds.size.y / 2 * (targetRatio / screenRatio);
        }
    }

    /* Moves the camera to @room */
    public void MoveToRoom(Transform room) 
    {
        StartCoroutine(Panning(new Vector3(room.position.x, room.position.y - this.shift, this.transform.position.z), this.panningDuration));
    }

    /* Coroutine: Moves camera from the current position to @targetPosition in @duration seconds */
    private IEnumerator Panning(Vector3 targetPosition, float duration)
    {
        Vector3 startingPosition = this.transform.position;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            this.transform.position = Vector3.Lerp(startingPosition, targetPosition, t/duration);
            yield return Time.deltaTime;
        }
        this.transform.position = targetPosition;
    }

    public void Shake()
    {
        Camera.main.DOShakePosition(
            shakeDuration,
            shakeStrength,
            shakeVibrato,
            shakeRandomness,
            shakeFadeOut,
            shakeRandomnessMode)
            .SetUpdate(UpdateType.Normal, true);
    }

    private IEnumerator ShakeRepeater()
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            Time.timeScale = 0.1f;
            Shake();
            yield return new WaitForSeconds(timeFreeze*Time.timeScale);
            Time.timeScale = 1;
            yield return new WaitForSeconds(3-timeFreeze);
        }
    }
}
