﻿using System.Collections.Generic;
using Wkb2Gltf;

namespace pg2b3dm;

public static class TileWriter
{
    public static byte[] ToTile(List<GeometryRecord> geometries, string copyright = "", bool addOutlines = false, double areaTolerance = 0.01, string defaultColor = "#FFFFFF", string defaultMetallicRoughness = "#008000", bool doubleSided = true, bool createGltf = false)
    {
        var triangles = GetTriangles(geometries, areaTolerance);
        var attributes = GetAttributes(geometries);

        var bytes = TileCreator.GetTile(attributes, triangles, copyright, addOutlines, defaultColor, defaultMetallicRoughness, doubleSided, createGltf);

        return bytes;
    }

    private static Dictionary<string, List<object>> GetAttributes(List<GeometryRecord> geometries)
    {
        var res = new Dictionary<string, List<object>>();

        foreach (var geom in geometries) {
            foreach (var attr in geom.Attributes) {
                if (!res.ContainsKey(attr.Key)) {
                    res.Add(attr.Key, new List<object> { attr.Value });
                }
                else {
                    res[attr.Key].Add(attr.Value);
                }
            }
        }
        return res;
    }

    private static List<List<Triangle>> GetTriangles(List<GeometryRecord> geomrecords, double areaTolerance = 0.01)
    {
        var triangles = new List<List<Triangle>>();
        foreach (var g in geomrecords) {
            var geomTriangles = new List<Triangle>() { };

            geomTriangles.AddRange(g.GetTriangles(areaTolerance));
            triangles.Add(geomTriangles);
        }

        return triangles;
    }
}
