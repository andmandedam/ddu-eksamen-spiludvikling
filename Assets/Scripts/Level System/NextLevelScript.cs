using UnityEngine;

public class NextLevelScript : MonoBehaviour
{
    public GameManager gameManager;
    public int playerLayer = 9;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == playerLayer) gameManager.NextLevel();
    }
}
