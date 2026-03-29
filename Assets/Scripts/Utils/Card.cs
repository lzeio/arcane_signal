using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image cardSprite;
    [SerializeField] private Sprite frontSprite;

    public int id = -1;

    [SerializeField] private float flipDuration = 0.15f;
    public bool IsFlipping => isFlipping;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFlipping) return;

        Flip(OnFlipComplete);
    }
    public void Init(Sprite sprite, int cardId)
    {
        id = cardId;
        cardSprite.sprite = sprite;
        frontSprite = sprite;
        Unflip(null, 3f);

    }

    private bool isFlipping = false;

    public void Flip(System.Action<Card> onComplete = null, float delay = 0f)
    {
        StartCoroutine(FlipRoutine(true, delay, onComplete));
    }

    public void Unflip(System.Action<Card> onComplete = null, float delay = 0f)
    {
        StartCoroutine(FlipRoutine(false, delay, onComplete));
    }


    private IEnumerator FlipRoutine(bool showFront, float delay, System.Action<Card> onComplete)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        if (!TryStartFlip())
            yield break;

        yield return AnimateScale(1f, 0f);

        SwapSprite(showFront);

        yield return AnimateScale(0f, 1f);

        EndFlip(onComplete);
    }

    private bool TryStartFlip()
    {
        if (isFlipping) return false;

        isFlipping = true;
        return true;
    }

    private IEnumerator AnimateScale(float from, float to)
    {
        for (float t = 0; t < flipDuration; t += Time.deltaTime)
        {
            float scale = Mathf.Lerp(from, to, t / flipDuration);
            transform.localScale = new Vector3(scale, 1f, 1f);
            yield return null;
        }

        transform.localScale = new Vector3(to, 1f, 1f);
    }

    private void SwapSprite(bool showFront)
    {
        cardSprite.sprite = showFront ? frontSprite : CardManager.Instance.GetBackSprite();
    }

    private void EndFlip(System.Action<Card> onComplete)
    {
        isFlipping = false;
        onComplete?.Invoke(this);
    }
    private void OnFlipComplete(Card card)
    {
        Debug.Log("Flip Complete: " + card.id);
        CardManager.OnCardClicked?.Invoke(card);
    }

    public void Matched()
    {
        cardSprite.enabled = false;
    }
}
