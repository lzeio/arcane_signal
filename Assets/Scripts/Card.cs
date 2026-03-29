using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image cardSprite;

    //can be set from Cardmanager as this wil save the reference to sprite for each card
    [SerializeField] private Sprite frontSprite;
    public int id = -1;

    [SerializeField] private float flipDuration = 0.15f;

    void OnEnable()
    {
        Unflip(null, 3f); // Start with cards face down
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFlipping) return;

        Flip(OnFlipComplete);
    }
    public void SetCardData(Sprite sprite, int cardId)
    {
        id = cardId;
        cardSprite.sprite = sprite;
        frontSprite = sprite;
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

        if (isFlipping)
            yield break;

        isFlipping = true;

        // Hide
        for (float t = 0; t < flipDuration; t += Time.deltaTime)
        {
            float scale = Mathf.Lerp(1f, 0f, t / flipDuration);
            transform.localScale = new Vector3(scale, 1f, 1f);
            yield return null;
        }

        transform.localScale = new Vector3(0f, 1f, 1f);

        // Swap
        cardSprite.sprite = showFront ? frontSprite : CardManager.Instance.backSprite;

        // Show
        for (float t = 0; t < flipDuration; t += Time.deltaTime)
        {
            float scale = Mathf.Lerp(0f, 1f, t / flipDuration);
            transform.localScale = new Vector3(scale, 1f, 1f);
            yield return null;
        }

        transform.localScale = Vector3.one;

        isFlipping = false;


        onComplete?.Invoke(this);
    }

    private void OnFlipComplete(Card card)
    {
        CardManager.OnCardsClicked?.Invoke(card);
    }

    public void Matched()
    {
        cardSprite.enabled = false;
    }
}
