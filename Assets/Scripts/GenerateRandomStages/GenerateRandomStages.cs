using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using System.Linq;


public class GenerateRandomStages : MonoBehaviour
{
	private string dirPath = "Scripts/GenerateRandomStages";
	public Tilemap[] tilePrefabs;
	public List<CharGameObjectPair> charGameObjectPair;
	private Dictionary<char, GameObject> charGameObjectMap;
	private int mapHeight = 14;
	//元となるテンプレートの数
	private int fileCount = 20;
	//つなげるファイル数
	private int totalFilesToCombine = 8;
	List<string> combinedLines = new List<string>();
	void Start()
	{
		GenerateStage();
	}

	public void GenerateStage()
	{
		MapAssociation();
		RenderMap();
	}

	public void MapAssociation()
	{

		AddTemplateFile("tmpl_start");

		List<int> shuffledIndices = new List<int>(Enumerable.Range(0, fileCount));
		System.Random rng = new System.Random();
		Debug.Log("~~~~~~~~~~~~~~~~~~~~");
		for (int i = 0; i < totalFilesToCombine; i++)
		{
			int index = shuffledIndices[rng.Next(shuffledIndices.Count)];
			shuffledIndices.Remove(index);
			Debug.Log($"tmpl_{index}.txt");
			AddTemplateFile("tmpl_" + index);
		}
		Debug.Log("~~~~~上記のファイル達を結合した~~~~~");

		AddTemplateFile("tmpl_end");

	}

	private void AddTemplateFile(string resourcePath)
	{
		TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);
		var reader = new StringReader(textAsset.text);


		var stringList = new List<string>();
		while (reader.Peek() != -1)
		{
			stringList.Add(reader.ReadLine());
		}
		var lines = stringList.ToArray();

		if (textAsset == null)
		{
			Debug.LogError($"{resourcePath} not found!");
			textAsset = Resources.Load<TextAsset>("tmpl_default");
		}
		if (combinedLines.Count == 0)
		{
			combinedLines.AddRange(lines);
		}
		else
		{
			for (int j = 0; j < lines.Length; j++)
			{
				if (lines.Length != mapHeight)
				{
					Debug.LogError("mapの縦の長さが正しくない");
					Debug.LogError($"{lines.Length}!={mapHeight}");
				}
				combinedLines[j] += lines[j];
			}
		}
	}

	void RenderMap()
	{
		int mapWidth = 10;
		//startとtitleのテンプレートの列の長さを足したもの
		int frontBackWidth = 16;
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < (totalFilesToCombine * mapWidth + frontBackWidth); x++)
			{
				char mapChar = combinedLines[y][x];
				if ('0' <= mapChar && mapChar <= '9')//tileの場合
				{
					Tilemap tilePrefab = GetTilemapPrefabForChar(mapChar);
					if (tilePrefab != null)
						Instantiate(tilePrefab, new Vector3(x, mapHeight - y - 1, 0), Quaternion.identity);
				}
				else if ('A' <= mapChar && mapChar <= 'Z')//障害物の場合
				{
					GameObject obstaclePrefab = GetObstaclePrefabForChar(mapChar);
					if (obstaclePrefab != null)
						Instantiate(obstaclePrefab, new Vector3(x, mapHeight - y - 1, 0), Quaternion.identity);
				}
			}
		}
	}

	Tilemap GetTilemapPrefabForChar(char c)
	{
		if (c == ' ' || c == '\t' || c == '\n')
			return null;

		int index = 0;
		index = c - '0' - 1; // 0から9までの文字に対応するプレハブを取得する場合
		if (index >= 0 && index < tilePrefabs.Length)
		{
			return tilePrefabs[index];
		}
		return null; // 該当するプレハブがない場合
	}

	GameObject GetObstaclePrefabForChar(char c)
	{
		if (c == ' ' || c == '\t' || c == '\n')
			return null;
		// ListからDictionaryへの変換
		charGameObjectMap = new Dictionary<char, GameObject>();
		foreach (var pair in charGameObjectPair)
		{
			charGameObjectMap[pair.character] = pair.gameObject;
		}
		//keyとvalueがあることをチェックする
		if (charGameObjectMap.ContainsKey(c) && charGameObjectMap[c] != null)
			return charGameObjectMap[c];
		return null;

	}
}
