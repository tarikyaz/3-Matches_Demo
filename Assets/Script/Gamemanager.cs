using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum MatchType
	{
	NotSelected,
	RandomMatches,
	ThreeMatches,
	FullMatch
	}

public enum GameMode
	{
	NotSelected,
	DestoyingGame,
	CompletingGame
	}

public class Gamemanager : MonoBehaviour {
	public static Gamemanager instance;
	public InGameMining InGameMiningUI;
	public StartMenuManager StartMenu;
	public HeaderScript Header;
	public int NumberOfMatches;
	public float ColoringSpeed = 0.5f;
	public CubeScript[,] CubesArrayGrid;
	public CubeScript[] CubesArray;
	public int CountY, CountX;
	public Color OriginalColor, ClickedColour, PreMatchColour, FinalMatchColor, FreezedColcor, TestColor, StoneColor;
	public MatchType matchingmode;
	public GameMode gamemode;
	//public CubeScript CubePrefab;


	private void Awake()
		{
		instance = this;
		}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void BuildingGridSystem(Transform CubesParent)
		{
		int locationX = new int();

		//setting up the nuber of x and y cubes
		CountY = CubesArray.Length / CubesParent.GetComponent<GridLayoutGroup>().constraintCount;
		CountX = CubesParent.GetComponent<GridLayoutGroup>().constraintCount;
		CubesArrayGrid = new CubeScript[CountY, CountX];

		//Setting up the grid system of cubes
		List<CubeScript> CubesSameLine;
		for (int i = 0; i < CountY; i++)
			{
			int ni = i;
			int NewStartJ = ni * CountX;
			int newEndJ = ni * CountX + CountX;
			CubesSameLine = new List<CubeScript>();
			locationX = 0;
			for (int j = NewStartJ; j < newEndJ; j++)
				{
				int nj = j;
				CubesSameLine.Add(CubesArray[nj]);
				//Setting Up the locations of cubes
				CubesArray[nj].LocationX = locationX;
				CubesArray[nj].LocationY = ni;
				locationX++;
				}

			// Adding the same line list to the grid cube list
			for (int k = 0; k < CubesSameLine.Count; k++)
				{
				CubesArrayGrid[i, k] = CubesSameLine[k];
				}
			//CubesArrayGrid.Add(CubesSameLine);
			}
		// setting up the lcoal nerby for each cube
		for (int i = 0; i < CubesArray.Length; i++)
			{
			int ni = i;
			CubesArray[ni].SettingUpLocalNerby();
			}
		}

	}
