using FableCraft;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UCM = UnityEngine.SceneManagement;
using System.Linq;

namespace FableCraft
{
    public class SceneManager : MonoBehaviour, ISceneManager
    {
        string beatName = "Please enter a name...";
        Scene _newScene;
    }
}