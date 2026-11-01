using UnityEngine;

public class ClickableEnemy : MonoBehaviour
{
    private EnemyTracker tracker;

    public void Initialize(EnemyTracker trackerReference)
    {
        tracker = trackerReference;
    }

    private void OnMouseDown()
    {
        if (tracker != null)
        {
            tracker.DefeatEnemy(gameObject);
        }
    }
}