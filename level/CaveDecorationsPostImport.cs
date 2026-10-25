#if TOOLS

using Godot;

namespace DwarfTossing.Level;

[Tool]
public partial class CaveDecorationsPostImport : EditorScenePostImport
{
    public override GodotObject _PostImport(Node scene)
    {
        _iterate(scene);
        return scene;
    }

    private static void _iterate(Node node)
    {
        if (node == null) return;

        _configureCollision(node);
            
        foreach (Node child in node.GetChildren())
        {
            _iterate(child);
        }
    }

    private static void _configureCollision(Node node)
    {
        if (!(node is StaticBody3D body && body.GetParent() is MeshInstance3D mesh))
            return;
        
        string name = mesh.Name.ToString().ToLower();
        if (name.Contains("-item"))
        {
            body.SetCollisionLayer(2);
            body.SetCollisionMask(0);
            
            mesh.SetName(name.Replace("-item", ""));
        }
        else if(name.Contains("-pickaxe"))
        {
            body.SetCollisionLayer(1 | 4);
            body.SetCollisionMask(1);
            
            mesh.SetName(name.Replace("-pickaxe", ""));
        }
    }
    
}
#endif
