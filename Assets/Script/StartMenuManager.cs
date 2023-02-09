using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class StartMenuManager : MonoBehaviour
	{
	public Button StartBtn,  GameTypeSelectorBtn, RandomMatchBtn, ThreeMatchBtn, FullMatchBtn, GameModeSelectorBtn, MiningGameBtn , MatchingGameBtn;
	public Button[] ButtonsList;
	public InputField ClickedCubesNumberInput;
	public int MaximumExtraCubes;
	public GameObject GameTypeDropGO, GameModeDropGO, MiningDropGO , MatchingDropGO;
	public Text GameTypeText,GameModeTypeTxt;
	public Toggle FreezedCubeToggle;
	public bool IsFreezedCube;
	public bool AllIsOK = true;

	// Use this for initialization
	void Start()
		{
		MaximumExtraCubes = 3 * Gamemanager.instance.InGameMiningUI.CubesParent.childCount / 2;
		ButtonsList = GetComponentsInChildren<Button>(true);
		
		FreezedCubeToggle.onValueChanged.AddListener(delegate
			{
				FreezedCubeToggleValueChanged(FreezedCubeToggle);
				});
		Gamemanager.instance.matchingmode = MatchType.NotSelected;
		GameTypeSelectorBtn.onClick.AddListener(() =>
		{
			if (GameTypeDropGO.gameObject.activeSelf)
				GameTypeDropGO.gameObject.SetActive(false);
			else if (!GameTypeDropGO.gameObject.activeSelf)
				GameTypeDropGO.gameObject.SetActive(true);
		});

		GameModeSelectorBtn.onClick.AddListener(() =>
		{
			if (GameModeDropGO.gameObject.activeSelf)
				GameModeDropGO.gameObject.SetActive(false);
			else if (!GameModeDropGO.gameObject.activeSelf)
				GameModeDropGO.gameObject.SetActive(true);
		});

		MiningGameBtn.onClick.AddListener(() =>
		{
			GameModeDropGO.gameObject.SetActive(false);
			if (!GameTypeSelectorBtn.gameObject.activeSelf)
				GameTypeSelectorBtn.gameObject.SetActive(true);
			if (MatchingDropGO.gameObject.activeSelf)
				MatchingDropGO.gameObject.SetActive(false);
			if (!MiningDropGO.gameObject.activeSelf)
				MiningDropGO.gameObject.SetActive(true);
			GameTypeText.text = "Select minig type";

		});

		MatchingGameBtn.onClick.AddListener(() =>
		{
			GameModeDropGO.gameObject.SetActive(false);
			if (!GameTypeSelectorBtn.gameObject.activeSelf)
				GameTypeSelectorBtn.gameObject.SetActive(true);
			if (MiningDropGO.gameObject.activeSelf)
				MiningDropGO.gameObject.SetActive(false);
			if (!MatchingDropGO.gameObject.activeSelf)
				MatchingDropGO.gameObject.SetActive(true);
			GameTypeText.text = "Select matching type";
		});

		ThreeMatchBtn.onClick.AddListener(() =>
		{
			Gamemanager.instance.matchingmode = MatchType.ThreeMatches;
			GameTypeText.text = ThreeMatchBtn.GetComponentInChildren<Text>().text + " mode";
			GameTypeDropGO.gameObject.SetActive(false);
		});

		RandomMatchBtn.onClick.AddListener(() =>
		{
			Gamemanager.instance.matchingmode = MatchType.RandomMatches;
			GameTypeText.text = RandomMatchBtn.GetComponentInChildren<Text>().text + " mode";
			GameTypeDropGO.gameObject.SetActive(false);
		});

		FullMatchBtn.onClick.AddListener(() =>
		{
			Gamemanager.instance.matchingmode = MatchType.FullMatch;
			GameTypeText.text = FullMatchBtn.GetComponentInChildren<Text>().text + " mode";
			GameTypeDropGO.gameObject.SetActive(false);
		});



		//CubesManager.instance.GameMode = GameMode.FullMatches;
		ClickedCubesNumberInput.placeholder.GetComponent<Text>().text = ClickedCubesNumberInput.placeholder.GetComponent<Text>().text + " between 0 and " + MaximumExtraCubes.ToString() + " ...";
		StartBtn.onClick.AddListener(() =>
		{
			if (!AllIsOK)
				AllIsOK = true;
			if (ClickedCubesNumberInput.text == "")
				StartCoroutine(NoticeMessage(ClickedCubesNumberInput, "Please insert number"));
			else
				{
				int TheInsertedCubesNumber;
				TheInsertedCubesNumber = int.Parse(ClickedCubesNumberInput.text);
				if (TheInsertedCubesNumber < 0 || TheInsertedCubesNumber > MaximumExtraCubes)
					StartCoroutine(NoticeMessage(ClickedCubesNumberInput, "The number must be between 0 & " + MaximumExtraCubes));
				}


			if (Gamemanager.instance.matchingmode == MatchType.NotSelected)
				StartCoroutine(NoticeMessage(GameTypeText, "Game mode not selected"));

			 if (!GameTypeSelectorBtn.gameObject.activeSelf)
				StartCoroutine(NoticeMessage(GameModeTypeTxt, "Select game mode"));

			if (AllIsOK)
				{
				Gamemanager.instance.InGameMiningUI.gameObject.SetActive(true);
				Gamemanager.instance.Header.gameObject.SetActive(true);
				ButtonsList = null;
				}
		});
		}

	private void FreezedCubeToggleValueChanged(Toggle newValue)
		{
		if (!Gamemanager.instance.InGameMiningUI.gameObject.activeSelf)
			IsFreezedCube = newValue.isOn;
		}

	IEnumerator NoticeMessage(InputField TheInput, object TheMessage)
		{
		if (GameModeDropGO.gameObject.activeSelf)
			GameModeDropGO.gameObject.SetActive(false);
		if (GameTypeDropGO.gameObject.activeSelf)
			GameTypeDropGO.gameObject.SetActive(false);
		for (int i = 0; i < ButtonsList.Length; i++)
			{
			ButtonsList[i].interactable = false;
			}

		if (AllIsOK)
			AllIsOK = false;
		
		InputField.ContentType TheOriginalContentType = TheInput.contentType;
		string TheOriginalTxt;
		int OrigianlCharacterLimit = TheInput.characterLimit;
		TheInput.readOnly = true;
		TheInput.characterLimit = 0;
		TheInput.contentType = InputField.ContentType.Standard;
		TheInput.lineType = InputField.LineType.MultiLineNewline;
		TheOriginalTxt = TheInput.text;
		for (int i = 0; i < 5; i++)
			{
			TheInput.text = TheMessage.ToString();
			yield return new WaitForSeconds(0.3f);
			TheInput.text = TheOriginalTxt;
			}

		TheInput.contentType = TheOriginalContentType;
		TheInput.characterLimit = OrigianlCharacterLimit;
		TheInput.readOnly = false;
		for (int i = 0; i < ButtonsList.Length; i++)
			{
			ButtonsList[i].interactable = true;
			}
		}
	IEnumerator NoticeMessage(Text TheText, object TheMessage)
		{
		if (GameModeDropGO.gameObject.activeSelf)
			GameModeDropGO.gameObject.SetActive(false);
		if (GameTypeDropGO.gameObject.activeSelf)
			GameTypeDropGO.gameObject.SetActive(false);
		for (int i = 0; i < ButtonsList.Length; i++)
			{
			ButtonsList[i].interactable = false;
			}
		if (AllIsOK)
			AllIsOK = false;
		string TheOriginalTxt;
		TheOriginalTxt = TheText.text;
		for (int i = 0; i < 5; i++)
			{
			TheText.text = TheMessage.ToString();
			yield return new WaitForSeconds(0.3f);
			TheText.text = TheOriginalTxt;
			}
		for (int i = 0; i < ButtonsList.Length; i++)
			{
			ButtonsList[i].interactable = true;
			}
		}
	}
