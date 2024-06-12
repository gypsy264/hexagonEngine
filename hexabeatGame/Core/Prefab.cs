using Project.Core;
using Project.Game;
using Project.Graphics;

using OpenTK.Mathematics;

public class Prefab : GameObject
{
    private GameObject _prefabInstance;

    

    public Prefab(string tag, Vector3 position, Vector3 scale, string texturePath, Renderer renderer, Engine engine, int layer = 0, Vector2? colliderSize = null)
    : base(tag, position, scale, texturePath, renderer, engine, layer, colliderSize)
    {
        _prefabInstance = this;
    }

    public GameObject Spawn(Vector3 position, Vector3 scale, int layer = 0, Vector2? colliderSize = null)
    {
        GameObject instance = new GameObject(_prefabInstance.Tag, position, scale, _prefabInstance.TexturePath, _prefabInstance.Renderer, _prefabInstance.Engine, layer, colliderSize);
        //instance.AddScript(new Player(this)); // assuming you want to add the same script as the prefab
        return instance;
    }
}