using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionManager : MonoBehaviour
{
    public OptionObject [] options;

    private void Awake()
    {
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (Input.anyKeyDown){
            foreach (OptionObject option in options)
            {
                if (Input.GetKeyDown(option.key))
                {
                    option.gameObject.SetActive(true);
                    Cursor.visible = option.isShowCursor;
                }
            }
        }
    }

    public void ChangedScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetMouseCurser(bool isShow)
    {
        Cursor.visible = isShow;
    }

    [System.Serializable]
    public class OptionObject
    {
        public GameObject gameObject;
        public KeyCode key;
        public bool isShowCursor;
    }
}
