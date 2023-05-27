using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaypointMovement))]
public class WaypointMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WaypointMovement waypoint = (WaypointMovement)target;
        if(GUILayout.Button("Get waypoints position"))
        {
            Vector3[] result = new Vector3[waypoint.onSceneWaypointsList.Length];
            for (int i = 0; i < waypoint.onSceneWaypointsList.Length; i++)
            {
                result[i] = waypoint.onSceneWaypointsList[i].position;
            }
            waypoint.waypointsPositionsList = result;
        }
    }
}
