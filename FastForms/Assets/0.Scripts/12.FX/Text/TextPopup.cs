using StdNounou;
using System;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    [SerializeField, ReadOnly] private float currentLifetime;

    [SerializeField] private TextMeshPro textMesh;

    public event Action<TextPopup> OnEnd;

    private Vector3 targetPosition;
    private int listIndex = 1;

    [SerializeField, ReadOnly] private SO_TextPopupData textPopupData;

    public static TextPopup Create(string text, Vector2 pos, SO_TextPopupData textPopupData)
    {
        TextPopup next = PoolsManager.Instance.TextPopupPool.GetNext(pos);
        next.Setup(text, textPopupData);
        return next;
    }

    private void Update()
    {
        ProcessLifetime();
        ProcessMovements();
        ProcessFade();
    }

    private void ProcessLifetime()
    {
        if (currentLifetime > 0)
        {
            currentLifetime -= Time.deltaTime * listIndex;
            return;
        }
        Kill();
    }

    private void ProcessMovements()
    {
        if (VectorUtils.Vector2ApproximatlyEquals(this.transform.position, targetPosition)) return;
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * textPopupData.TravelSpeed);
    }

    private void ProcessFade()
    {
        float currentLifetimePercentage = currentLifetime / textPopupData.Lifetime * 100;
        if (currentLifetimePercentage >= textPopupData.AlphaFadeLifetimeStart) return;

        this.textMesh.alpha = Mathf.Lerp(textMesh.alpha, 0, Time.deltaTime * textPopupData.FadeSpeed * listIndex);
    }

    public void Setup(string text, SO_TextPopupData textPopupData, int listIndex = 1)
    {
        this.listIndex = listIndex;
        textMesh.alpha = 1;
        this.textPopupData = textPopupData;
        textMesh.SetText(text);
        currentLifetime = textPopupData.Lifetime;
        targetPosition = textPopupData.TargetPosition + this.transform.position;
    }

    public void SetListIndex(int index)
        => this.listIndex = index;

    public void Kill()
    {
        OnEnd?.Invoke(this);
        PoolsManager.Instance.TextPopupPool.Enqueue(this);
    }

    public void AddToTargetPosition(Vector3 pos)
    {
        targetPosition += pos;
    }

    private void OnDestroy()
    {
        OnEnd?.Invoke(this);
    }
}