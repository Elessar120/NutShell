using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HazardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject smallFish;
    [SerializeField] private GameObject BigFish;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private List<Sprite> smallFishes;
    [SerializeField] private List<Sprite> bigFishes;

    private float timeSinceLastHazard;
    private float timeSinceStart;
    private float nextHazardTimeRatio = 0.5f;
    private void Start()
    {
        InitData();
    }

    public void Reset()
    {
        timeSinceStart = 0;
        timeSinceLastHazard = 0;
        while (parentTransform.childCount > 0)
        {
            DestroyImmediate(parentTransform.GetChild(0).gameObject);
        }
    }
    private void Update()
    {
        if (!GameManager.instance.isPlaying)
            return;
        timeSinceStart += Time.deltaTime;
        timeSinceLastHazard += Time.deltaTime;
        GenerateNewHazard();
    }

    private void GenerateNewHazard()
    {
        var newHazard = GetHazard();
        if (newHazard == null)
        {
            return;
        }
        timeSinceLastHazard = 0;
        nextHazardTimeRatio = Random.Range(0, 1.0f);
        GameObject hazard = null;
        if (newHazard.Value.type == HazardType.SmallFish)
        {

           hazard = Instantiate(smallFish, parentTransform);
           hazard.GetComponent<Image>().sprite = smallFishes[Random.Range(0, smallFishes.Count)];
           SetPatrol(hazard,newHazard.Value.xPos,newHazard.Value.range,newHazard.Value.speed);
        }
        else if (newHazard.Value.type == HazardType.BigFish)
        {

            hazard = Instantiate(BigFish, parentTransform); 
            hazard.GetComponent<Image>().sprite = bigFishes[Random.Range(0, bigFishes.Count)];
            SetPatrol(hazard,newHazard.Value.xPos,newHazard.Value.range,newHazard.Value.speed);

        }
        else if(newHazard.Value.type == HazardType.Platform)
        {

            hazard = Instantiate(platform, parentTransform); 

        }
        hazard.GetComponent<RectTransform>().anchoredPosition = new Vector2(newHazard.Value.xPos, 1250);
        hazard.SetActive(true);
    }

    private void SetPatrol(GameObject hazard,float xPos, float range,float speed)
    {
        var patrol = hazard.GetComponent<Patrol>();
        patrol.SetDirection(Random.Range(0,2) == 0);
        patrol.range = range; //Random.Range(100, 450);
        patrol.speed = speed; ;
        patrol.centerPosition = xPos;//;
        patrol.xPos = xPos;
    }

    private (HazardType type,float xPos,float range,float speed)? GetHazard()
    {
        if (timeSinceLastHazard < 0.01)
        {
            return null;
        }
        var row = GetDataRow(timeSinceStart);
        if (row == null) return null;
        if (timeSinceLastHazard < row.RespawnMinInterval) return null;
        if ((timeSinceLastHazard - row.RespawnMinInterval) <
            (row.RespawnMaxInterval - row.RespawnMinInterval) * nextHazardTimeRatio) return null;
        var type = GenerateHazardType(row);
        if (type == HazardType.None) return null;
        return (type, Random.Range(-250, 250), 
                Random.Range(100f, 200f * row.LifeHazardsRange),
                Random.Range(100f, 200f * row.LifeHazardsSpeed));
    }

    private HazardType GenerateHazardType(GameSettingRow row)
    {
        var sumProb = row.PlatformProbability + row.SmallFishProbability + row.BigFishProbability;
        if (sumProb <= 0.0001f) return HazardType.None;
        var rnd = Random.Range(0, sumProb);
        if (row.PlatformProbability > 0.0001f && rnd <= row.PlatformProbability) return HazardType.Platform;
        if (row.SmallFishProbability > 0.0001f && rnd <= row.PlatformProbability + row.SmallFishProbability) return HazardType.SmallFish;
        return HazardType.BigFish;
    }

    private GameSettingRow GetDataRow(float f)
    {
        for (var i=0;i<settingData.Count;i++)
            if (settingData[i].MatchTime(f))
                return settingData[i];
        return null;
    }

    private void InitData()
    {
        
        settingData.Add(new GameSettingRow(0,26,3,8,0,1,2, 1,3)); //No Spawn
        settingData.Add(new GameSettingRow(26,52,3,7,5,0.7f,2,1f,3)); //No Spawn
        settingData.Add(new GameSettingRow(52, 70,2,7,4,1,1,1,3)); //Platform Only
        settingData.Add(new GameSettingRow(70,78,3,7,3,2,2,1,2));
        settingData.Add(new GameSettingRow(78,90,3,6,4,2,2,1,2));
        settingData.Add(new GameSettingRow(90,95,3,7,4,3f,2,1,2));
        settingData.Add(new GameSettingRow(95,128,3,8,3,3,4,1,1.5f));
        settingData.Add(new GameSettingRow(128,162,3,3,0,3f,4f,1,1.5f));
        settingData.Add(new GameSettingRow(162,193,3,3,0,2f,4f,.5f,1));
        settingData.Add(new GameSettingRow(193,197,3,3,3,2.5f,5f,.5f,1));
        
        
        settingData.Add(new GameSettingRow(0,26)); //No Spawn
        settingData.Add(new GameSettingRow(26,52,1,0,0,3,6,2f,4)); //No Spawn
        settingData.Add(new GameSettingRow(52, 70,0,1,0,3,4,2,4)); //Platform Only
        settingData.Add(new GameSettingRow(70,78,0,0,0,3,3,1,3));
        settingData.Add(new GameSettingRow(78,90,1,1,0,3,3,2,4));
        settingData.Add(new GameSettingRow(90,95,1,1,0,3.5f,4,2,4));
        settingData.Add(new GameSettingRow(95,128,1,1,0,4,5,2,4));
        settingData.Add(new GameSettingRow(128,162,1,1,0,4.5f,5.5f,2,4));
        settingData.Add(new GameSettingRow(162,193,1,1,0,5f,5f,2,4));
        settingData.Add(new GameSettingRow(193,197,1,1,1,5,4f,1,3));
        
        
        settingData.Add(new GameSettingRow(197,232, 1, 1,1 , 5, 1.5f, 2,4)); //No Spawn
        settingData.Add(new GameSettingRow(232,240,1,0,0,5,2,1f,3)); //No Spawn
        settingData.Add(new GameSettingRow(240, 249,0,1,0,5,3,2,4)); //Platform Only
        settingData.Add(new GameSettingRow(249,301,0,1,1,5f,2,1,3));
        settingData.Add(new GameSettingRow(301,335,1,1,0,3,6,1,4));
        settingData.Add(new GameSettingRow(335,370,1,0,1,4f,4,1,4));
        settingData.Add(new GameSettingRow(370,386,1,1,1,3,5,2,4));
        settingData.Add(new GameSettingRow(386,454,1,1,1,3f,6f,1,3));
        settingData.Add(new GameSettingRow(454,471,1,1,0,2.5f,7f,1,3));
        settingData.Add(new GameSettingRow(471,505,1,1,1,3,8f,1,3));
        
        
        settingData.Add(new GameSettingRow(505,520)); //No Spawn
        settingData.Add(new GameSettingRow(520,538,1,1,1,3,6,1f,3)); //No Spawn
        settingData.Add(new GameSettingRow(538, 570,0,1,1,3,6,1,2)); //Platform Only
        settingData.Add(new GameSettingRow(570,588,1,1,0,3,6,1,2));
        settingData.Add(new GameSettingRow(588,615,1,1,0,3,6,1,1.5f));
        settingData.Add(new GameSettingRow(615,646,0,1,0,3.5f,7,1,1.5f));
        settingData.Add(new GameSettingRow(646,654,1,1,0,4,8,1,1.25f));
        settingData.Add(new GameSettingRow(654,667,0,0,1,4.5f,6f,.5f,3));
        settingData.Add(new GameSettingRow(667,675,1,1,1,6f,6f,.5f,2.5f));
        settingData.Add(new GameSettingRow(675,680,1,0,0,6,6f,.5f,2));
        settingData.Add(new GameSettingRow(680,693,1,1,1,6,6f,.5f,1));
    }

    private readonly List<GameSettingRow> settingData = new List<GameSettingRow>();
    
    class GameSettingRow
    {
        private readonly float minTime, maxTime;
        public readonly float PlatformProbability, SmallFishProbability, BigFishProbability;
        public readonly float LifeHazardsSpeed, LifeHazardsRange;
        public readonly float RespawnMinInterval, RespawnMaxInterval;

        public bool MatchTime(float x)
        {
            return x>= minTime && x <= maxTime;
        }
        public GameSettingRow(float minTime, float maxTime, float pPlatform=0, float pSmall=0, float pBig=0,float speed=0,float range=0,float minInterval=0,float maxInterval=0)
        {
            this.minTime = minTime;
            this.maxTime = maxTime;
            PlatformProbability = pPlatform;
            SmallFishProbability = pSmall;
            BigFishProbability = pBig;
            LifeHazardsSpeed = speed;
            LifeHazardsRange = range;
            RespawnMinInterval = minInterval;
            RespawnMaxInterval = maxInterval;
        }
    }
    enum HazardType
    {
        Platform,
        SmallFish,
        BigFish,
        None
    }
}
