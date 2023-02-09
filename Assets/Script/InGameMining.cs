using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



public class InGameMining : MonoBehaviour
	{
	public bool DoChange = true;
	public float ElementFadingDouration;
	public Text NextMatchesText, CorrentMatchesText, PreviewMatchesText;
	public GameObject RandomModeGO;
	public Transform CubesParent;
	public List<CubeScript> PreMatchList;
	public bool FinalMatchFound;
	public Image FadingImage;

	private void Awake()
		{

		Gamemanager.instance.CubesArray = new CubeScript[CubesParent.childCount];
		for (int i = 0; i < CubesParent.childCount; i++)
			{
			int ni = i;
			Gamemanager.instance.CubesArray[ni] = CubesParent.GetChild(ni).GetComponent<CubeScript>();
			}
		}

	private void OnEnable()
		{
		StartingUp();
		}

	void StartingUp()
		{
		Gamemanager.instance.Header.GameModeText.text = Gamemanager.instance.StartMenu.GameTypeText.text;
		if (Gamemanager.instance.matchingmode == MatchType.ThreeMatches)
			Gamemanager.instance.NumberOfMatches = 3;
		if (Gamemanager.instance.matchingmode == MatchType.RandomMatches)
			RandomModeGO.gameObject.SetActive(true);
		FadingImage.gameObject.SetActive(true);
		FadingImage.DOFade(0, 2).OnComplete(() => { FadingImage.gameObject.SetActive(false); });
		if (Gamemanager.instance.matchingmode == MatchType.RandomMatches)
			{
			RandomModeGO.gameObject.SetActive(true);
			ChangeTheNumberOfMatches();
			}
		Gamemanager.instance.BuildingGridSystem(CubesParent);
		StartCoroutine(SettingUpStaticCubes());
		}

	public void CubeClicked(CubeScript TheClickedCube)
		{
		//adding  to the clicked list
		SetAsClickedCube(TheClickedCube);
		FinalMatchFound = false;
		MatchingManager(TheClickedCube);
		if (RandomModeGO.gameObject.activeSelf && FinalMatchFound)
			ChangeTheNumberOfMatches();
		Debug.Log("Prematch cleared");
		PreMatchList.Clear();
		}

	public void CubeUnclicked(CubeScript TheUnclickedCube)
		{
		RemoveFromClickedCube(TheUnclickedCube);

		// if the cube was prematch remove it
		if (PreMatchList.Contains(TheUnclickedCube))
			RemoveFromPrematch(TheUnclickedCube);

		//removing the nearby cubes
		RemovingTheNearClicked(TheUnclickedCube);
		}


	void RemovingTheNearClicked(CubeScript TheUncLickedCube)
		{
		bool NearbyMatchFound = true;

		for (int i = 0; i < TheUncLickedCube.LocalNerby.Count; i++)
			{
			//int ni = i;
			if (TheUncLickedCube.LocalNerby[i].IsClciked)
				{
				//cheeking if there are no nerby prematch before removing the cube from the CubeMatches
				NearbyMatchFound = false;
				for (int j = 0; j < TheUncLickedCube.LocalNerby[i].LocalNerby.Count; j++)
					{
					//int nj = j;
					if (PreMatchList.Contains(TheUncLickedCube.LocalNerby[i].LocalNerby[j]))
						NearbyMatchFound = true;
					}

				if (!NearbyMatchFound)
					{
					// removing the clicked cube from the prematch list
					RemoveFromPrematch(TheUncLickedCube.LocalNerby[i]);
					}
				}
			}
		}

	void MatchingManager(CubeScript TheCube)
		{
		if (Gamemanager.instance.matchingmode == MatchType.RandomMatches)
			Gamemanager.instance.NumberOfMatches = int.Parse(CorrentMatchesText.text);

		Debug.Log(TheCube + " cheacking");
		for (int i = 0; i < TheCube.LocalNerby.Count; i++)
			{
			if (!TheCube.LocalNerby[i].IsFinalMatch && !TheCube.LocalNerby[i].IsFreezed && (TheCube.LocalNerby[i].IsClciked || TheCube.LocalNerby[i].IsStone))
				{
				Debug.Log(TheCube.LocalNerby[i] + " is prematch");
				// Add to prematch list
				SetAsPreMatch(TheCube);

				// Add the found local to prematch list
				if (PreMatchList.Count < Gamemanager.instance.NumberOfMatches || (PreMatchList.Count < 2 && Gamemanager.instance.matchingmode == MatchType.FullMatch))
					{
					SetAsPreMatch(TheCube.LocalNerby[i]);
					CubeScript TheNewCube;
					TheNewCube = TheCube.LocalNerby[i];
					// looping over locals
					Debug.Log("Looping over " + TheCube.LocalNerby[i]);
					for (int j = 0; j < TheNewCube.LocalNerby.Count; j++)
						{
						Debug.Log("		looping over locals");
						if (PreMatchList.Count >= Gamemanager.instance.NumberOfMatches && Gamemanager.instance.matchingmode != MatchType.FullMatch)
							{
							Debug.Log("PreMatchList.Count " + PreMatchList.Count);
							Debug.Log("Break");
							break;
							}
						// if there's prematch around the cube
						else if (!TheNewCube.LocalNerby[j].IsFinalMatch && !TheCube.LocalNerby[i].IsFreezed && !PreMatchList.Contains(TheNewCube.LocalNerby[j]) && (TheNewCube.LocalNerby[j].IsClciked || TheNewCube.LocalNerby[j].IsStone))
							{
							Debug.Log("there's prematch " + TheNewCube.LocalNerby[j] + " around the cube");
							MatchingManager(TheNewCube.LocalNerby[j]);
							}
						else
							{
							Debug.Log("		" + TheNewCube.LocalNerby[j] + "Not prematch");
							}
						}
					}
				// Final match found
				if (PreMatchList.Count >= Gamemanager.instance.NumberOfMatches || Gamemanager.instance.matchingmode == MatchType.FullMatch)
					{
					SetAsFinalMatch(PreMatchList);
					FinalMatchFound = true;
					Debug.Log("final match created");
					break;
					}
				}
			else
				{
				Debug.Log(TheCube.LocalNerby[i] + " is not prematch");
				}
			}

		}

	void Coloring(CubeScript Thecube, Color TheColor)
		{
		Thecube.GetComponent<Image>().DOColor(TheColor, Gamemanager.instance.ColoringSpeed);
		}

	public void ChangeTheNumberOfMatches()
		{
		Debug.LogWarning("ChangeCalled");
		if (string.IsNullOrEmpty(NextMatchesText.text) && string.IsNullOrEmpty(CorrentMatchesText.text) && string.IsNullOrEmpty(PreviewMatchesText.text))
			{
			NextMatchesText.text = GetRandomNumber();
			CorrentMatchesText.text = GetRandomNumber(NextMatchesText.text);

			}
		else
			{
			PreviewMatchesText.text = CorrentMatchesText.text;
			CorrentMatchesText.text = NextMatchesText.text;
			NextMatchesText.text = GetRandomNumber(PreviewMatchesText.text, CorrentMatchesText.text);
			}
		}

	IEnumerator SettingUpStaticCubes()
		{
		for (int i = 0; i < int.Parse(Gamemanager.instance.StartMenu.GetComponent<StartMenuManager>().ClickedCubesNumberInput.text); i++)
			{
			int RandomX = Random.Range(0, Gamemanager.instance.CountX);
			int RandomY = Random.Range(0, Gamemanager.instance.CountY);
			if (Gamemanager.instance.StartMenu.IsFreezedCube)
				{
				Gamemanager.instance.CubesArrayGrid[RandomY, RandomX].IsFreezed = true;
				Gamemanager.instance.CubesArrayGrid[RandomY, RandomX].GetComponent<Image>().DOColor(Gamemanager.instance.FreezedColcor, ElementFadingDouration);
				}
			else if (!Gamemanager.instance.StartMenu.IsFreezedCube)
				{
				Gamemanager.instance.CubesArrayGrid[RandomY, RandomX].GetComponent<Image>().DOColor(Gamemanager.instance.StoneColor, ElementFadingDouration);
				Gamemanager.instance.CubesArrayGrid[RandomY, RandomX].IsStone = true;

				}
			yield return new WaitForSecondsRealtime(0.001f);
			}
		}

	void SetAsFinalMatch(List<CubeScript> TheList)
		{
		for (int i = 0; i < TheList.Count; i++)
			{
			//int ni = i;
			CubeScript TheCube = TheList[i];
			//adding to final match list
			if (!TheCube.IsFinalMatch)
				{

				TheCube.IsFinalMatch = true;

				//changing color
				Coloring(TheCube, Gamemanager.instance.FinalMatchColor);

				}
			//removing from clicked cubes
			TheCube.IsClciked = false;
			}
		}

	void SetAsPreMatch(CubeScript TheCube)
		{

		if (!PreMatchList.Contains(TheCube))
			{
			Coloring(TheCube, Gamemanager.instance.PreMatchColour);
			PreMatchList.Add(TheCube);
			}
		}

	void RemoveFromPrematch(CubeScript TheCube)
		{

		if (PreMatchList.Contains(TheCube))
			{
			PreMatchList.Remove(TheCube);

			if (!TheCube.IsClciked)
				Coloring(TheCube, Gamemanager.instance.OriginalColor);
			else if (TheCube.IsClciked)
				Coloring(TheCube, Gamemanager.instance.ClickedColour);
			}
		}

	void RemoveFromClickedCube(CubeScript TheCube)
		{
		if (TheCube.IsClciked)
			{
			TheCube.IsClciked = false;
			Coloring(TheCube, Gamemanager.instance.OriginalColor);
			}
		}

	void SetAsClickedCube(CubeScript TheCube)
		{
		TheCube.IsClciked = true;
		Coloring(TheCube, Gamemanager.instance.ClickedColour);
		}

	private string GetRandomNumber(string exeptionTxt)
		{
		int exeption = int.Parse(exeptionTxt);
		int number;
		number = Random.Range(2, 9);
		while (number == exeption)
			number = Random.Range(2, 9);

		return number.ToString();
		}

	private string GetRandomNumber(string exeptionTxt1, string exeptionTxt2)
		{
		int exeption1 = int.Parse(exeptionTxt1);
		int exeption2 = int.Parse(exeptionTxt2);
		int number;
		number = Random.Range(2, 9);
		while (number == exeption1 || number == exeption2)
			number = Random.Range(2, 9);

		return number.ToString();
		}
	private string GetRandomNumber()
		{
		int number;
		number = Random.Range(2, 9);
		return number.ToString();
		}
	}
