#if TOOLS

using Godot;

namespace DwarfTossing.Character.Player;

[Tool]
public partial class PlayerModelPostImport : EditorScenePostImport
{
    public override GodotObject _PostImport(Node scene)
    {
        _iterate(scene, true);
        return scene;
    }

    private static void _iterate(Node node, bool inHead)
    {
        if (node == null) return;

        if (!inHead)
        {
            string name = node.Name.ToString();
            inHead = name.StartsWith("Head") || name.StartsWith("Neck");
        }

        if (inHead)
        {
            _configureModel(node);
        }
        
        foreach (Node child in node.GetChildren())
        {
            _iterate(child, inHead);
        }
    }

    private static void _configureModel(Node node)
    {
        if (node is MeshInstance3D meshInstance3D)
        {
            meshInstance3D.SetLayerMask(2);
        }
    }
    
}
#endif
