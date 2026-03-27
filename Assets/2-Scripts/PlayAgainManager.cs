using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayAgainManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(LoadFirstLevel());
    }
    IEnumerator LoadFirstLevel()
    {
        yield return new WaitForSecondsRealtime(1f);
        
        FindFirstObjectByType<GameSession>().ResetGameSession();
        FindFirstObjectByType<ScenePersist>().ResetScenePersist();
        
    }
}
