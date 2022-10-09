using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace Devshifu.CrossPromo
{
    public class SettingsWindow : EditorWindow
    {
        private Vector2 scrollPosition = Vector2.zero;
        private int dataCount;
        private CrossPromoData crossPromoData = new CrossPromoData();

        [MenuItem("Window/Devshifu/Cross Promo", false, 20)]
        private static void Init()
        {
            SettingsWindow window = (SettingsWindow)GetWindow(typeof(SettingsWindow));
            window.titleContent = new GUIContent("Cross Promo");
            window.minSize = new Vector2(520, 520);
            window.Show();
        }

        private void OnEnable()
        {
            if (AssetDatabase.IsValidFolder("Assets/CrossPromo/PromoFiles"))
            {
                if (File.Exists(Application.dataPath + "/CrossPromo/PromoFiles/" + "CrossPromoData.json"))
                {
                    string json = File.ReadAllText(Application.dataPath + "/CrossPromo/PromoFiles/" + "CrossPromoData.json");
                    if (json.Length > 0)
                        crossPromoData = JsonUtility.FromJson<CrossPromoData>(json);
                }
            }
        }

        private void OnGUI()
        {
            EditorStyles.label.wordWrap = true;
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(position.width), GUILayout.Height(position.height));
            EditorGUILayout.LabelField("Cross Promo Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            crossPromoData.IsEnabled = EditorGUILayout.Toggle("IsEnabled", crossPromoData.IsEnabled);

            ShowSettings();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUILayout.EndScrollView();
        }

        private void ShowSettings()
        {
            Color defaultColor = GUI.color;
            Color blackColor = new Color(0.65f, 0.65f, 0.65f, 1);
            GUI.color = blackColor;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;

            if (GUILayout.Button("Add new game data"))
            {
                crossPromoData.Games.Add(new GameItem());
            }

            for (int i = 0; i < crossPromoData.Games.Count; i++)
            {
                crossPromoData.Games[i].AppTitle = EditorGUILayout.TextField("Game Title", crossPromoData.Games[i].AppTitle);

                EditorGUILayout.BeginHorizontal();
                crossPromoData.Games[i].IconUrl = EditorGUILayout.TextField("Icon link", crossPromoData.Games[i].IconUrl);
                if (GUILayout.Button("Test", GUILayout.Width(60)))
                {
                    Application.OpenURL(crossPromoData.Games[i].IconUrl);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                crossPromoData.Games[i].PageUrl = EditorGUILayout.TextField("Store link", crossPromoData.Games[i].PageUrl);
                if (GUILayout.Button("Test", GUILayout.Width(60)))
                {
                    Application.OpenURL(crossPromoData.Games[i].PageUrl);
                }
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Remove", GUILayout.Height(30)))
                {
                    crossPromoData.Games.RemoveAt(i);
                }
            }

            EditorGUILayout.EndVertical();
            GUI.color = defaultColor;

            EditorGUILayout.Space();

            if (crossPromoData.Games.Count > 0)
            {
                if (GUILayout.Button("Generate file", GUILayout.Height(60)))
                {
                    GenerateFile(crossPromoData);
                }
            }
        }

        private void GenerateFile(CrossPromoData promoData)
        {
            if (!AssetDatabase.IsValidFolder("Assets/CrossPromo/PromoFiles"))
            {
                AssetDatabase.CreateFolder("Assets/CrossPromo", "PromoFiles");
                AssetDatabase.Refresh();
            }

            string json = JsonUtility.ToJson(promoData);
            File.WriteAllText(Application.dataPath + "/CrossPromo/PromoFiles/" + "CrossPromoData.json", json);
            AssetDatabase.Refresh();
        }
    }
}