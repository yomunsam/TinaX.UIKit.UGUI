using TinaX.UIKit.UGUI.ConfigAssets;
using TinaX.UIKit.UGUI.Consts;
using TinaXEditor.Core;
using TinaXEditor.Core.Consts;
using TinaXEditor.Core.Utils.Localization;
using UnityEditor;
using UnityEngine;

namespace TinaXEditor.UIKit.UGUI.ProjectSetting
{
    public class ProjectSettingsProvider
    {
        private static StyleDefine _styles;
        private static StyleDefine Styles
        {
            get
            {
                if (_styles == null)
                    _styles = new StyleDefine();
                return _styles;
            }
        }

        private static Localizer _localizer;
        private static Localizer L
        {
            get
            {
                if( _localizer == null )
                        _localizer = new Localizer();
                return _localizer;
            }
        }

        private static bool _refresh = false;
        private static UIKitUGUIConfigAsset _configAsset;
        private static SerializedObject _configAssetSerializedObject;

        private static SerializedProperty SPCreateUICameraAutomatically;
        private static SerializedProperty SPCanvasScalerConfig;
        private static SerializedProperty SPCanvasScalerConfig_UICanvasScalerMode;

        private static bool _showCanvasScalerFoldout;

        [SettingsProvider]
        public static SettingsProvider GetUIKitUGUISettingsProvider()
            => new SettingsProvider($"{XEditorConsts.ProjectSettingsRootName}/UIKit/UGUI", SettingsScope.Project, new string[] { "TinaX", "UIKit", "uGUI" })
            {
                label = "UGUI",
                guiHandler = (searchContent) =>
                {
                    if (!_refresh)
                        RefreshData();

                    EditorGUILayout.BeginVertical(Styles.Body);
                    if (_configAsset == null)
                    {
                        GUILayout.Label(L.NoConfig);
                        if (GUILayout.Button(L.CreateConfigAsset, GUILayout.MaxWidth(120)))
                        {
                            EditorConfigAsset.CreateConfigIfNotExists<UIKitUGUIConfigAsset>(UIKitUGUIConsts.DefaultConfigAssetName);
                            RefreshData();
                        }
                    }
                    else
                    {
                        GUILayout.Space(10);
                        EditorGUILayout.PropertyField(SPCreateUICameraAutomatically, L.CreateUICameraAutomatically);
                        if(SPCreateUICameraAutomatically.boolValue)
                        {
                            EditorGUILayout.PropertyField(_configAssetSerializedObject.FindProperty("UICameraConfigAssets"), L.UICameraConfigAssets);
                        }

                        //自动创建UI EventSystem
                        GUILayout.Space(10);
                        EditorGUILayout.PropertyField(_configAssetSerializedObject.FindProperty("CreateUIEventSystemAutomatically"), L.CreateUIEventSystemAutomatically);

                        GUILayout.Space(10);
                        _showCanvasScalerFoldout = EditorGUILayout.Foldout(_showCanvasScalerFoldout, L.CanvasScalerConfig);
                        if (_showCanvasScalerFoldout)
                        {
                            EditorGUILayout.PropertyField(SPCanvasScalerConfig_UICanvasScalerMode, new GUIContent("UI Scale Mode"));
                            GUILayout.Space(10);

                            if(SPCanvasScalerConfig_UICanvasScalerMode.enumValueIndex == (int)UnityEngine.UI.CanvasScaler.ScaleMode.ConstantPixelSize)
                            {
                                EditorGUILayout.PropertyField(SPCanvasScalerConfig.FindPropertyRelative("UIScaleFactor"), new GUIContent("Scale Factor"));
                            }
                            else if(SPCanvasScalerConfig_UICanvasScalerMode.enumValueIndex == (int)UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize)
                            {
                                EditorGUILayout.PropertyField(SPCanvasScalerConfig.FindPropertyRelative("ReferenceResolution"));
                                EditorGUILayout.PropertyField(SPCanvasScalerConfig.FindPropertyRelative("ScreenMatchMode"));
                                EditorGUILayout.PropertyField(SPCanvasScalerConfig.FindPropertyRelative("CanvasScalerMatchWidthOrHeight"));
                            }
                            else if(SPCanvasScalerConfig_UICanvasScalerMode.enumValueIndex == (int)UnityEngine.UI.CanvasScaler.ScaleMode.ConstantPhysicalSize)
                            {
                                EditorGUILayout.PropertyField(SPCanvasScalerConfig.FindPropertyRelative("PhySicalUnit"));
                                EditorGUILayout.PropertyField(SPCanvasScalerConfig.FindPropertyRelative("FallbackScreenDPI"));
                                EditorGUILayout.PropertyField(SPCanvasScalerConfig.FindPropertyRelative("DefaultSpriteDPI"));
                            }

                            EditorGUILayout.PropertyField(SPCanvasScalerConfig.FindPropertyRelative("ReferencePixelsPerUnit"));
                        }

                    }
                    EditorGUILayout.EndVertical();


                    _configAssetSerializedObject?.ApplyModifiedProperties();
                },
                deactivateHandler = () =>
                {
                    if (_configAsset != null)
                    {
                        EditorUtility.SetDirty(_configAsset);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
            };

        private static void RefreshData()
        {
            _configAsset = EditorConfigAsset.GetConfig<UIKitUGUIConfigAsset>(UIKitUGUIConsts.DefaultConfigAssetName);
            if (_configAsset != null)
            {
                _configAssetSerializedObject = new SerializedObject(_configAsset);
                SPCreateUICameraAutomatically = _configAssetSerializedObject.FindProperty("CreateUICameraAutomatically");
                SPCanvasScalerConfig = _configAssetSerializedObject.FindProperty("CanvasScalerConfig");
                SPCanvasScalerConfig_UICanvasScalerMode = SPCanvasScalerConfig.FindPropertyRelative("UICanvasScalerMode");
            }

            _refresh = true;
        }


        class StyleDefine
        {
            public StyleDefine()
            {
                Body = new GUIStyle();
                Body.padding.left = 15;
                Body.padding.right = 15;

                Title = new GUIStyle(EditorStyles.largeLabel);
            }

            public GUIStyle Body;

            public GUIStyle Title;
        }

        class Localizer
        {
            bool IsHans = EditorLocalizationUtil.IsHans();
            bool IsJp = EditorLocalizationUtil.IsJapanese();

            public string NoConfig
            {
                get
                {
                    if (IsHans)
                        return "请创建TinaX UIKit(uGUI)配置文件.";
                    if (IsJp)
                        return "TinaX UIKit(uGUI) 構成ファイルを作成してください。";
                    return "Please create a TinaX UIKit(uGUI) configuration asset.";
                }
            }

            public string CreateConfigAsset
            {
                get
                {
                    if (IsHans)
                        return "创建配置资产";
                    if (IsJp)
                        return "構成アセットを作成する";
                    return "Create Configuration Asset.";
                }
            }

            private GUIContent _CreateUICameraAutomatically;
            public GUIContent CreateUICameraAutomatically
            {
                get
                {
                    if(_CreateUICameraAutomatically == null)
                    {
                        if (IsHans)
                            _CreateUICameraAutomatically = new GUIContent("自动创建UICamera", "自动创建UICamera");
                        else if (IsJp)
                            _CreateUICameraAutomatically = new GUIContent("UIカメラを自動的に作成する", "UIカメラを自動的に作成する");
                        else
                            _CreateUICameraAutomatically = new GUIContent("Create UICamera Automatically", "Create UICamera Automatically");
                    }

                    return _CreateUICameraAutomatically;
                }
            }

            private GUIContent _CreateUIEventSystemAutomatically;
            public GUIContent CreateUIEventSystemAutomatically
            {
                get
                {
                    if (_CreateUIEventSystemAutomatically == null)
                    {
                        if (IsHans)
                            _CreateUIEventSystemAutomatically = new GUIContent("自动创建EventSystem", "自动创建EventSystem");
                        else if (IsJp)
                            _CreateUIEventSystemAutomatically = new GUIContent("EventSystem自動的に作成する", "EventSystem自動的に作成する");
                        else
                            _CreateUIEventSystemAutomatically = new GUIContent("Create EventSystem Automatically", "Create EventSystem Automatically");
                    }

                    return _CreateUIEventSystemAutomatically;
                }
            }

            public GUIContent _UICameraConfigAsset;
            public GUIContent UICameraConfigAssets
            {
                get
                {
                    if (_UICameraConfigAsset == null)
                    {
                        if (IsHans)
                            _UICameraConfigAsset = new GUIContent("UICamera配置资产", "根据此处配置自动创建UICamera");
                        else if (IsJp)
                            _UICameraConfigAsset = new GUIContent("UICamera構成アセット", "ここの構成に従ってUICameraを自動的に作成します");
                        else
                            _UICameraConfigAsset = new GUIContent("UICamera configuration assets", "Automatically create UICamera according to the configuration here");
                    }

                    return _UICameraConfigAsset;
                }
            }

            public GUIContent _CanvasScalerConfig;
            public GUIContent CanvasScalerConfig
            {
                get
                {
                    if (_CanvasScalerConfig == null)
                    {
                        if (IsHans)
                            _CanvasScalerConfig = new GUIContent("UGUI CanvasScaler设置");
                        else if (IsJp)
                            _CanvasScalerConfig = new GUIContent("UGUI CanvasScalerの設定");
                        else
                            _CanvasScalerConfig = new GUIContent("UGUI CanvasScaler settings");
                    }

                    return _CanvasScalerConfig;
                }
            }
        }
    }
}
