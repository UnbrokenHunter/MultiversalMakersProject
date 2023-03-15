using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static MultiversalMakers.LevelGenerator.ObjectData;

namespace MultiversalMakers
{
    public class LevelGenerator : MonoBehaviour
    {

        #region Setup and Variables

        // Map
        [SerializeField, PreviewField(500), HideLabel] private Texture2D map;

        // Tile Objects
        [SerializeField, TableList()] private ObjectData[] objects;
        private readonly Dictionary<Color, Varient> objectNames = new();

        // Tilemaps
        private Tilemap collisionLayer;
        private Tilemap deathLayer;

        // Death
        private Dictionary<Vector3Int, Varient> deaths = new();
        [PreviewField(200), LabelWidth(100), SerializeField] private TileBase[] deathTiles;

        // Ladders
        private Dictionary<Vector3Int, GameObject> ladders = new();

        // Lamp
        private Dictionary<Vector3Int, Varient> lamps = new();

        // Gates and Keys
        private GameObject GateAndKey1;
        private GameObject GateAndKey2;
        private GameObject GateAndKey3;

        // Swap
        private GameObject swap1Obj;
        private bool swap1Spawned = false;

        private void Awake()
        {
            print("awake");
            foreach (ObjectData obj in objects)
                Array.ForEach(
                    obj.varients,
                    varient => objectNames.Add(RoundColor(varient.varientColor), varient));

        }

        private void Start()
        {
            SetupLevel();

            GenerateLevel();
            
            SetCamera();
        }

        private Color RoundColor(Color color) => new(
                    (float)Math.Round(color.r, 2),
                    (float)Math.Round(color.g, 2),
                    (float)Math.Round(color.b, 2),
                    (float)Math.Round(color.a, 2));

        void SetupLevel()
        {
            // Tilemaps
            gameObject.AddComponent<Grid>();

            GameObject _collisionTilemapObject = Instantiate(new GameObject("Collision"), transform);
            collisionLayer = _collisionTilemapObject.AddComponent<Tilemap>();
            _collisionTilemapObject.AddComponent<TilemapRenderer>();
            _collisionTilemapObject.AddComponent<TilemapCollider2D>();

            GameObject _deathTilemapObject = Instantiate(new GameObject("Death"), transform);
            _deathTilemapObject.layer = 8;
            deathLayer = _deathTilemapObject.AddComponent<Tilemap>();
            _deathTilemapObject.AddComponent<TilemapRenderer>();
            _deathTilemapObject.AddComponent<TilemapCollider2D>();

        }

        void SetCamera()
        {
            Transform cam = Camera.main.transform;
            cam.position = new Vector3(map.width / 2, map.height / 2, cam.position.z);
        }
        
        #endregion

        void GenerateLevel()
        {
            for (int x = 0; x < map.width; x++)
                for (int y = 0; y < map.height; y++)
                    GenerateTile(x, y);

            SpawnDeathTiles();
            SpawnLamps();
        }

        void GenerateTile(int x, int y)
        {
            // Get the data from this tile
            Color pixel = RoundColor(map.GetPixel(x, y));

            #region Null Checks
            // No data on this tile
            if (pixel.a == 0) return;
            // Unknown Tile
            if (objectNames.ContainsKey(pixel) == false) return;
            #endregion

            // Set Data
            Vector3Int _position = new(x, y);
            Varient _varient = objectNames[pixel];

            if (_varient.tileObject != null)
            {
                if (_varient.name == "Red Tile" || _varient.name == "Blue Tile")
                    GenerateCollisionTile(_position, _varient);

                else if (
                    _varient.name == "Red Death Up" || _varient.name == "Blue Death Up" ||
                    _varient.name == "Red Death Down" || _varient.name == "Blue Death Down" ||
                    _varient.name == "Red Death Left" || _varient.name == "Blue Death Left" ||
                    _varient.name == "Red Death Right" || _varient.name == "Blue Death Right")
                    GenerateDeathTile(_position, _varient);


            }

            else if (_varient.summonObject != null)
            {
                if (_varient.name == "Red Player" || _varient.name == "Blue Player")
                    GenerateObject(_position, _varient);

                else if (_varient.name == "Red Collectable" || _varient.name == "Blue Collectable")
                    GenerateObject(_position, _varient);

                else if (_varient.name == "Red Win" || _varient.name == "Blue Win")
                    GenerateObject(_position, _varient);

                else if (_varient.name == "Ladder")
                    GenerateLadder(_position, _varient);

                else if (_varient.name == "Swap Places 1")
                    SetSwapPlaces(_position, _varient);

                else if (_varient.name == "Orange Lamp" || _varient.name == "Blue Lamp")
                    GenerateLamps(_position, _varient);

                else if (_varient.name is 
                    "Gate 1" or "Key 1" or
                    "Gate 2" or "Key 2" or
                    "Gate 3" or "Key 3")
                    GenerateGatesAndKeys(_position, _varient);

            }

        }

        #region Object Behavior

        void GenerateCollisionTile(Vector3Int _position, Varient _obj) => collisionLayer.SetTile(_position, _obj.tileObject);
        void GenerateDeathTile(Vector3Int _position, Varient _obj) => deaths.Add(_position, _obj);
        void SpawnDeathTiles()
        {
            Vector3Int checkPos(Vector3Int pos, int x, int y)
            {
                return new Vector3Int(pos.x + x, pos.y + y, pos.z);
            }

            foreach (Vector3Int pos in deaths.Keys)
            {
                // Red
                if (deaths[pos].varientColor == new Color(0.8470589f, 0.2627451f, 0.08235294f, 1))
                {
                    // Red Down
                    if (collisionLayer.HasTile(checkPos(pos, 0, -1)))
                    {
                        deathLayer.SetTile(pos, deathTiles[0]);
                    }

                    // Red Up
                    else if (collisionLayer.HasTile(checkPos(pos, 0, 1)))
                    {
                        deathLayer.SetTile(pos, deathTiles[2]);
                    }

                    // Red Left
                    else if(collisionLayer.HasTile(checkPos(pos, 1, 0)))
                    {
                        deathLayer.SetTile(pos, deathTiles[4]);
                    }

                    // Red Right
                    else if(collisionLayer.HasTile(checkPos(pos, -1, 0)))
                    {
                        deathLayer.SetTile(pos, deathTiles[6]);
                    }
                }
                else
                {

                    // Blue Down
                    if (collisionLayer.HasTile(checkPos(pos, 0, -1)))
                        deathLayer.SetTile(pos, deathTiles[1]);


                    // Blue Up
                    else if(collisionLayer.HasTile(checkPos(pos, 0, 1)))
                        deathLayer.SetTile(pos, deathTiles[3]);


                    // Blue Left
                    else if(collisionLayer.HasTile(checkPos(pos, 1, 0)))
                        deathLayer.SetTile(pos, deathTiles[5]);


                    // Blue Right
                    else if(collisionLayer.HasTile(checkPos(pos, -1, 0)))
                        deathLayer.SetTile(pos, deathTiles[7]);
                }
            }
        }
        void GenerateObject(Vector3Int _position, Varient _obj) => Instantiate(_obj.summonObject, _position, Quaternion.identity, transform);
        void GenerateLadder(Vector3Int _position, Varient _obj)
        {
            // Extend Ladder
            if(RoundColor(map.GetPixel(_position.x, _position.y - 1)) == RoundColor(_obj.varientColor))
            {
                Vector3Int lastLadder = new (_position.x, _position.y - 1);
                SpriteRenderer renderer = ladders[lastLadder].GetComponent<SpriteRenderer>();

                renderer.size = new Vector2(renderer.size.x, renderer.size.y + 1);
                ladders.Add(_position, ladders[lastLadder]);
            }
            // Create New
            else
            {
                GameObject newLadder = Instantiate(_obj.summonObject, _position + (Vector3.right / 2), Quaternion.identity, transform);
                ladders.Add(_position, newLadder);
            }
        }
        void SetSwapPlaces(Vector3Int _position, Varient _obj)
        {
            if(swap1Spawned == false)
            {
                swap1Obj = Instantiate(_obj.summonObject, Vector3.zero, Quaternion.identity, transform);
                swap1Obj.transform.GetChild(0).transform.position = _position;

                swap1Spawned = true;
            }
            else
            {
                swap1Obj.transform.GetChild(1).transform.position = _position;
            }
        }
        void GenerateLamps(Vector3Int _position, Varient _obj) => lamps.Add(_position, _obj);
        void SpawnLamps()
        {
            Vector3Int checkPos(Vector3Int pos, int x, int y)
            {
                return new Vector3Int(pos.x + x, pos.y + y, pos.z);
            }

            foreach (Vector3Int pos in lamps.Keys)
            {

                Vector3 newPos = pos + new Vector3 (0.5f, 0.5f);

                // Red
                if (lamps[pos].varientColor == new Color(1f, 0.9607844f, 0.6156863f, 1))
                {
                    // Red Down
                    if (collisionLayer.HasTile(checkPos(pos, 0, -1)))
                    {
                        Instantiate(lamps[pos].summonObject, newPos, Quaternion.identity, transform);
                    }

                    // Red Up
                    else if (collisionLayer.HasTile(checkPos(pos, 0, 1)))
                    {
                        Instantiate(lamps[pos].summonObject, newPos, Quaternion.Euler(0, 0, 180), transform);
                    }

                    // Red Left
                    else if (collisionLayer.HasTile(checkPos(pos, 1, 0)))
                    {
                        Instantiate(lamps[pos].summonObject, newPos, Quaternion.Euler(0, 0, 90), transform);
                    }

                    // Red Right
                    else if (collisionLayer.HasTile(checkPos(pos, -1, 0)))
                    {
                        Instantiate(lamps[pos].summonObject, newPos, Quaternion.Euler(0, 0, -90), transform);
                    }
                }
                else
                {

                    // Blue Down
                    if (collisionLayer.HasTile(checkPos(pos, 0, -1)))
                    {
                        Instantiate(lamps[pos].summonObject, newPos, Quaternion.identity, transform);
                    }

                    // Blue Up
                    else if (collisionLayer.HasTile(checkPos(pos, 0, 1)))
                    {
                        Instantiate(lamps[pos].summonObject, newPos, Quaternion.Euler(0, 0, 180), transform);
                    }

                    // Blue Left
                    else if (collisionLayer.HasTile(checkPos(pos, 1, 0)))
                    {
                        Instantiate(lamps[pos].summonObject, newPos, Quaternion.Euler(0, 0, 90), transform);
                    }

                    // Blue  Right
                    else if (collisionLayer.HasTile(checkPos(pos, -1, 0)))
                    {
                        Instantiate(lamps[pos].summonObject, newPos, Quaternion.Euler(0, 0, -90), transform);
                    }
                }
            }
        }
        void GenerateGatesAndKeys(Vector3Int _position, Varient _obj)
        {

            Vector3Int checkPos(Vector3Int pos, int x, int y)
            {
                return new Vector3Int(pos.x + x, pos.y + y, pos.z);
            }

            Vector3 newPos = _position + new Vector3(0.5f, 0.5f);


            if (_obj.name == "Gate 1" || _obj.name == "Key 1")
            {
                if(GateAndKey1 == null)
                    GateAndKey1 = Instantiate(_obj.summonObject, Vector3.zero, Quaternion.identity, transform);

                if (_obj.name == "Key 1")
                    GateAndKey1.transform.GetChild(0).transform.position = newPos;

                if (_obj.name == "Gate 1")
                    GateAndKey1.transform.GetChild(1).transform.position = newPos;
            }

            if (_obj.name == "Gate 2" || _obj.name == "Key 2")
            {
                if (GateAndKey2 == null)
                    GateAndKey2 = Instantiate(_obj.summonObject, Vector3.zero, Quaternion.identity, transform);

                if (_obj.name == "Key 2")
                    GateAndKey2.transform.GetChild(0).transform.position = newPos;

                if (_obj.name == "Gate 2")
                    GateAndKey2.transform.GetChild(1).transform.position = newPos;
            }

            if (_obj.name == "Gate 3" || _obj.name == "Key 3")
            {
                if (GateAndKey3 == null)
                    GateAndKey3 = Instantiate(_obj.summonObject, Vector3.zero, Quaternion.identity, transform);

                if (_obj.name == "Key 3")
                    GateAndKey3.transform.GetChild(0).transform.position = newPos;

                if (_obj.name == "Gate 3")
                    GateAndKey3.transform.GetChild(1).transform.position = newPos;
            }

        }

        #endregion

        [System.Serializable]
        internal class ObjectData
        {
            [SerializeField, LabelText("Tile")] internal Varient[] varients = new Varient[2];

            [System.Serializable]
            internal class Varient
            {
                [SerializeField] internal string name;
                public Color varientColor;

                private enum ObjectTypes { Object, Tile };
                [SerializeField] private ObjectTypes objectType;

                [ShowIf("@objectType == ObjectTypes.Tile"), PreviewField(200), LabelWidth(100)] public TileBase tileObject;
                [ShowIf("@objectType == ObjectTypes.Object"), PreviewField(200), LabelWidth(100)] public GameObject summonObject;
            }
        }
    }
}
