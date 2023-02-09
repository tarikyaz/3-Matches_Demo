using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class HeaderScript : MonoBehaviour {
	public Button BackBtn;
	public Text GameModeText;
	// Use this for initialization
	void Start () {
		BackBtn.onClick.AddListener(() =>
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		});
		}
	
	// Update is called once per frame
	void Update () {
		
	}
}
