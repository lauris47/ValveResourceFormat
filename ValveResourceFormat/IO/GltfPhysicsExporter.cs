using System.Collections.Generic;
using System.Numerics;
using SharpGLTF.IO;
using SharpGLTF.Schema2;
using ValveResourceFormat.ResourceTypes;
using ValveResourceFormat.Serialization;
using Mesh = SharpGLTF.Schema2.Mesh;

namespace ValveResourceFormat.IO
{
    public static class GltfPhysicsExporter
    {

        public static Node AddModelPhys(ModelRoot exportedModel, IVisualNodeContainer parentNode, string name,
            IDictionary<string, Mesh> loadedMeshDictionary,
            Model model = null)
        {
            var phys = model.GetEmbeddedPhys();
            if (phys == null)
            {
                return null;
            }

            var physName = string.Concat(name, ".PHY");
            var physNode = parentNode.CreateNode(physName);

            var shapeTypeNames = new string[] { "Sphere", "Capsule", "Hull", "Mesh" };
            int shapeTypeCount = shapeTypeNames.Length;
            var groupCount = phys.CollisionAttributes.Count * shapeTypeCount;
            var verts = new List<Vector3>[groupCount];
            var inds = new List<int>[groupCount];
            for (var i = 0; i < groupCount; i++)
            {
                verts[i] = new();
                inds[i] = new();
            }

            CreatePhysGltfMeshes(phys, verts, inds);
            for (var i = 0; i < groupCount; i++)
            {
                if (verts[i].Count < 1 || inds[i].Count < 1)
                {
                    continue;
                }

                var attributes = phys.CollisionAttributes[i / shapeTypeCount];
                var tags = attributes.GetArray<string>("m_InteractAsStrings")
                    ?? attributes.GetArray<string>("m_PhysicsTagStrings");
                var group = attributes.GetStringProperty("m_CollisionGroupString");
                var groupName = $"{string.Join("_", tags)}";
                if (group != null)
                {
                    groupName = $"{groupName}{(groupName.Length > 1 ? "." : "")}{group}";
                }

                var physMeshName = string.Format("{0}.Mesh_{1}", physName, i);
                loadedMeshDictionary.TryGetValue(physMeshName, out var mesh);
                if (mesh == null)
                {
                    mesh = exportedModel.CreateMesh(physMeshName);
                    mesh.Name = physMeshName;

                    var primitive = mesh.CreatePrimitive();
                    var accessors = new Dictionary<string, Accessor>();
                    accessors["POSITION"] = GltfModelExporter.CreateAccessor(exportedModel, verts[i].ToArray());
                    foreach (var (attributeKey, accessor) in accessors)
                    {
                        primitive.SetVertexAccessor(attributeKey, accessor);
                    }
                    primitive.WithIndicesAccessor(PrimitiveType.TRIANGLES, inds[i]);

                    loadedMeshDictionary.Add(physMeshName, mesh);
                }
                var meshNode = physNode.CreateNode($"{physName}.{shapeTypeNames[i % shapeTypeCount]}.{groupName}"
                    ).WithMesh(mesh);
                var props = new Dictionary<string, object>();
                props["PHY.Shape"] = shapeTypeNames[i % shapeTypeCount];
                meshNode.Extras = JsonContent.Serialize(props);
            }

            GltfModelExporter.DebugValidateGLTF();
            return physNode;
        }

        private static void CreatePhysGltfMeshes(PhysAggregateData phys, List<Vector3>[] verts, List<int>[] inds)
        {
            for (var p = 0; p < phys.Parts.Length; p++)
            {
                var shape = phys.Parts[p].Shape;
                //TODO: Spheres
                //TODO: Capsules
                // Hulls
                foreach (var hull in shape.Hulls)
                {
                    var collisionAttributeIndex = hull.CollisionAttributeIndex;
                    collisionAttributeIndex = collisionAttributeIndex * 4 + 2;

                    var vertOffset = verts[collisionAttributeIndex].Count;
                    foreach (var v in hull.Shape.VertexPositions)
                    {
                        // if (bindPose.Any())
                        // {
                        //     v = Vector3.Transform(v, bindPose[p]);
                        // }

                        verts[collisionAttributeIndex].Add(v);
                    }

                    foreach (var face in hull.Shape.Faces)
                    {
                        var startEdge = face.Edge;
                        for (var edge = hull.Shape.Edges[startEdge].Next; edge != startEdge;)
                        {
                            var nextEdge = hull.Shape.Edges[edge].Next;
                            if (nextEdge == startEdge)
                            {
                                break;
                            }

                            inds[collisionAttributeIndex].Add(vertOffset + hull.Shape.Edges[startEdge].Origin);
                            inds[collisionAttributeIndex].Add(vertOffset + hull.Shape.Edges[edge].Origin);
                            inds[collisionAttributeIndex].Add(vertOffset + hull.Shape.Edges[nextEdge].Origin);
                            edge = nextEdge;
                        }
                    }
                }
                // Mesh
                foreach (var mesh in shape.Meshes)
                {
                    var collisionAttributeIndex = mesh.CollisionAttributeIndex;
                    collisionAttributeIndex = collisionAttributeIndex * 4 + 3;

                    var vertOffset = verts[collisionAttributeIndex].Count;
                    foreach (var v in mesh.Shape.Vertices)
                    {
                        // if (bindPose.Any())
                        // {
                        //     v = Vector3.Transform(vec, bindPose[p]);
                        // }

                        verts[collisionAttributeIndex].Add(v);
                    }

                    foreach (var tri in mesh.Shape.Triangles)
                    {
                        inds[collisionAttributeIndex].Add(vertOffset + tri.Indices[0]);
                        inds[collisionAttributeIndex].Add(vertOffset + tri.Indices[1]);
                        inds[collisionAttributeIndex].Add(vertOffset + tri.Indices[2]);
                    }
                }
            }
        }
    }
}
