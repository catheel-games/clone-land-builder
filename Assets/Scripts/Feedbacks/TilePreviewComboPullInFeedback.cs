using DG.Tweening;
using UnityEngine;

public class TilePreviewComboPullInFeedback : MonoBehaviour
{
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private float shardMaxHeight = 0.6f;
    [SerializeField] private Transform shardPrefab;

    private Sequence pullInSequence;

    public void Play(GameObject mainVisual, TilePreviewStar[] stars)
    {
        pullInSequence = DOTween.Sequence();

        mainVisual.SetActive(false);

        float linearCoefficient = 4 * shardMaxHeight / duration;
        float quadraticCoefficient = -linearCoefficient / duration;

        foreach(TilePreviewStar star in stars)
        {
            if (star != null)
            {
                Vector3 startPosition = Hexagons.HexToWorld(star.SideCoords);
                Vector3 endPosition = transform.position;

                Transform shardInstance = Instantiate(shardPrefab, startPosition, Quaternion.identity, transform);

                float time = 0f;

                Tween shardTween = DOTween.To(() => time, value => time = value, duration, duration);

                shardTween.OnUpdate(() =>
                {
                    float normalizedValue = time / duration;
                    float verticalOffset = (quadraticCoefficient * time + linearCoefficient) * time;
                    Vector3 basePosition = Vector3.Lerp(startPosition, endPosition, normalizedValue);

                    shardInstance.position = basePosition + Vector3.up * verticalOffset;
                });

                shardTween.OnComplete(() =>
                {
                    Destroy(shardInstance.gameObject);
                });

                pullInSequence.Join(shardTween);
            }
        }

        pullInSequence.OnComplete(() =>
        {
           mainVisual.SetActive(true); 
        });
    }

    public void Kill()
    {
        pullInSequence.Kill();
    }
}
