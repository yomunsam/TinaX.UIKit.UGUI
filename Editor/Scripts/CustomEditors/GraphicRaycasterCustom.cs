//using TinaX;
//using TinaX.UIKit.UGUI.MultipleDisplay.Components;
//using TinaX.UIKit.UGUI.Page.View;
//using TinaXEditor.Core.Utils.Localization;
//using UnityEditor;
//using UnityEngine;

//namespace TinaXEditor.UIKit.UGUI.CustomEditors
//{
//    [CustomEditor(typeof(UnityEngine.UI.GraphicRaycaster),false)]
//    [CanEditMultipleObjects]
//    public class GraphicRaycasterCustom : Editor
//    {
//        private StyleDefine _styles;
//        private StyleDefine Styles
//        {
//            get
//            {
//                if (_styles == null)
//                    _styles = new StyleDefine();
//                return _styles;
//            }
//        }

//        private Localizer _localizer;
//        private Localizer L
//        {
//            get
//            {
//                if (_localizer == null)
//                    _localizer = new Localizer();
//                return _localizer;
//            }
//        }

//        private UnityEngine.UI.GraphicRaycaster _target;

//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();

//            if (Application.isPlaying)
//                return;

//            if(_target == null)
//            {
//                _target = (UnityEngine.UI.GraphicRaycaster)target;
//            }

//            if (_target.gameObject.GetComponent<UGUIPageViewComponent>() != null)
//            {
//                EditorGUILayout.Space();
//                if (GUILayout.Button(L.ReplaceComponent))
//                {
//                    var gameObject = _target.gameObject;
//                    gameObject.GetComponentOrAdd<MultipleDisplayGraphicRaycaster>();
//                    Object.DestroyImmediate(_target);
//                }
//            }

//        }



//        class StyleDefine
//        {
//            public StyleDefine()
//            {
                
//            }

            
//        }

//        class Localizer
//        {
//            bool? _isHans;
//            bool IsHans
//            {
//                get
//                {
//                    if(_isHans == null)
//                        _isHans = EditorLocalizationUtil.IsHans();
//                    return _isHans.Value;
//                }
//            }

//            bool? _isJp;
//            bool IsJp
//            {
//                get
//                {
//                    if (_isJp == null)
//                        _isJp = EditorLocalizationUtil.IsJapanese();
//                    return _isJp.Value;
//                }
//            }

//            //上面为啥要这么折腾呢，因为智障Unity不让在构造函数里调用这些玩意，所以惰性加载

            

            
//            private GUIContent _ReplaceComponent;
//            public GUIContent ReplaceComponent
//            {
//                get
//                {
//                    if (_ReplaceComponent == null)
//                    {
//                        if (IsHans)
//                            _ReplaceComponent = new GUIContent("替换为TinaX的组件", "如果您需要在多个屏幕使用UI，请将该组件替换成TinaX的同类组件");
//                        else
//                            _ReplaceComponent = new GUIContent("Replace with Tinax component", "If you need to use the UI on multiple screens, replace this component with a similar component of tinax");
//                    }

//                    return _ReplaceComponent;
//                }
//            }
            
//        }
//    }
//}
