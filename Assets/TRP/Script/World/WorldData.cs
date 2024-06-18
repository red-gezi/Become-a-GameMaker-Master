using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class WorldManager
{
    public class WorldData
    {
        public List<Cube> cubes;
        public List<PortalCube> portalList;
        public WorldData(List<GameObject> worldCubes, List<PortalCube> portalList)
        {
            cubes = worldCubes.Select(cube => new Cube(cube)).ToList();
            this.portalList = portalList;
        }
        public class Cube
        {
            public int x;
            public int y;
            public int itemID;

            public Cube(GameObject cubeObject)
            {
                (x, y, _) = cubeObject.transform.position.ToInt3();
                itemID = cubeObject.GetComponent<GameCube>().CubeID;
            }
        }
        public class PortalCube
        {
            public int x;
            public int y;
            public string portalTag;
        }
    }
}
