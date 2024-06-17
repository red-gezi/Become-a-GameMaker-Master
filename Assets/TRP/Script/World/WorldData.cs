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
            public int z;
            public string itemTag;

            public Cube(GameObject cubeObject)
            {
                (x, y, z) = cubeObject.transform.position.ToInt3();
                itemTag = cubeObject.GetComponent<GameCube>().itemTag;
            }
        }
        public class PortalCube
        {
            public int x;
            public int y;
            public int z;
            public string portalTag;
        }
    }
}
