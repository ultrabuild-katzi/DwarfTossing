#if TOOLS

using Godot;

namespace DwarfTossing.Level.Cave;

[Tool]
public partial class CaveDecorationsPostImport : EditorScenePostImport
{
    public override GodotObject _PostImport(Node scene)
    {
        _iterate(scene, scene);
        return scene;
    }

    private static void _iterate(Node node, Node sceneRoot)
    {
        if (node == null) return;

        _configureCollision(node, sceneRoot);
            
        foreach (Node child in node.GetChildren())
        {
            _iterate(child, sceneRoot);
        }
    }

    private static void _configureCollision(Node node, Node sceneRoot)
    {
        string currentName = node.Name.ToString().ToLower();
        if (node is MeshInstance3D meshInstance && currentName.Contains("-area"))
        {
            Area3D area3D = new();
            area3D.Name = currentName.Replace("-area", "");
            area3D.CollisionLayer = 1 << 4;
            area3D.CollisionMask = 1 << 3;
            
            area3D.Transform = meshInstance.Transform; 
            
            CollisionShape3D collisionShape = new();
            collisionShape.Name = "CollisionShape3D";
            collisionShape.Shape = meshInstance.Mesh.CreateConvexShape(); 
            
            area3D.AddChild(collisionShape);
            node.ReplaceBy(area3D);
            
            area3D.Owner = sceneRoot;
            collisionShape.Owner = sceneRoot;
            
            meshInstance.QueueFree();
            
            return;
        }
        
        if (!(node is StaticBody3D body && body.GetParent() is MeshInstance3D mesh))
            return;
        
        string name = mesh.Name.ToString().ToLower();
        if (name.Contains("-item"))
        {
            body.SetCollisionLayer(2);
            body.SetCollisionMask(1);
            
            mesh.SetName(name.Replace("-item", ""));
        }
        else if (name.Contains("elevator_button"))
        {
            body.SetCollisionLayer(1 | 2);
            body.SetCollisionMask(0);
        } else if (name.Contains("detector_area"))
        {
            body.SetCollisionLayer(5);
            body.SetCollisionMask(0);
        }
        else
        {
            body.SetCollisionMask(0);
        }
        
    }
    
}
#endif
