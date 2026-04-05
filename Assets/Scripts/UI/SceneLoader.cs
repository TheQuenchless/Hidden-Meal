using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    
    public void Loadscene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
