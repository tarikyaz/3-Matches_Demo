using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeScript : MonoBehaviour
	{
	public int LocationX;
	public int LocationY;
	public List<CubeScript> LocalNerby;
	public bool IsStone, IsFreezed, IsClciked,IsFinalMatch;
	// Use this for initialization
	private void Awake()
		{
		gameObject.GetComponent<Button>().onClick.AddListener(() =>
		{
			if (!IsFinalMatch && !IsFreezed && !IsClciked && !IsStone)
				Gamemanager.instance.InGameMiningUI.CubeClicked(gameObject.GetComponent<CubeScript>());
			else if (IsClciked)
				Gamemanager.instance.InGameMiningUI.CubeUnclicked(gameObject.GetComponent<CubeScript>());
		});
		}

	public void SettingUpLocalNerby()
		{
		int NearX1 = new int();
		int NearX2 = new int();
		if (LocationX == 0)
			{
			NearX1 = 1;
			NearX2 = 1;
			}
		else if (LocationX > 0 && LocationX < Gamemanager.instance.CountX - 1)
			{
			NearX1 = LocationX - 1;
			NearX2 = LocationX + 1;
			}
		else if (LocationX == Gamemanager.instance.CountX - 1)
			{
			NearX1 = LocationX - 1;
			NearX2 = LocationX - 1;
			}
		int NearY1 = new int();
		int NearY2 = new int();

		if (LocationY == 0)
			{
			NearY1 = 1;
			NearY2 = 1;
			}
		else if (LocationY > 0 && LocationY < Gamemanager.instance.CountY - 1)
			{
			NearY1 = LocationY - 1;
			NearY2 = LocationY + 1;
			}

		else if (LocationY == Gamemanager.instance.CountY - 1)
			{
			NearY1 = LocationY - 1;
			NearY2 = LocationY - 1;
			}

		LocalNerby.Add(Gamemanager.instance.CubesArrayGrid[NearY1,LocationX]);
		if (NearY1 != NearY2)
			LocalNerby.Add(Gamemanager.instance.CubesArrayGrid[NearY2,LocationX]);
		LocalNerby.Add(Gamemanager.instance.CubesArrayGrid[LocationY,NearX1]);
		if (NearX1 != NearX2)
			LocalNerby.Add(Gamemanager.instance.CubesArrayGrid[LocationY,NearX2]);
		}
	}
