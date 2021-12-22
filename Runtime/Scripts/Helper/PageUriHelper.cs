using TinaX.UIKit.UGUI.Consts;

namespace TinaX.UIKit.UGUI.Helper
{
#nullable enable
    public static class PageUriHelper
    {
        static readonly string _uiKit_UGUI_Scheme;
        static readonly string _uiKit_UGUI_Scheme_Lower;
        static readonly int _uiKit_UGUI_Scheme_Length;

        static PageUriHelper()
        {
            _uiKit_UGUI_Scheme = $"{UIKitUGUIConsts.SchemeName}://";
            _uiKit_UGUI_Scheme_Lower = _uiKit_UGUI_Scheme.ToLower();
            _uiKit_UGUI_Scheme_Length = _uiKit_UGUI_Scheme.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="withoutScheme"></param>
        /// <returns></returns>
        public static bool IsMatch(string source, out string? withoutScheme)
        {
            if(source.ToLower().StartsWith(_uiKit_UGUI_Scheme_Lower))
            {
                withoutScheme = source.Substring(_uiKit_UGUI_Scheme_Length, source.Length - _uiKit_UGUI_Scheme_Length);
                return true;
            }
            withoutScheme = null;
            return false;
        }

        public static bool IsMatch(string source)
        {
            return source.ToLower().StartsWith(_uiKit_UGUI_Scheme_Lower);
        }

        /// <summary>
        /// 给定的是相对路径
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsRelativePath(string source)
        {
            return source.StartsWith("./") || source.StartsWith("../");
        }

        /// <summary>
        /// 拼接相对路径
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SpliceRelativePath(string sourceUri, string relativePath)
        {
            if(relativePath.StartsWith("./"))
                return SpliceRelativePath(sourceUri, relativePath.Substring(2));

            if(relativePath.StartsWith("../"))
                return SpliceRelativePath(GetParent(sourceUri), relativePath.Substring(3));

            if(relativePath.StartsWith("/"))
                return SpliceRelativePath(sourceUri, relativePath.Substring(1));

            if (sourceUri.EndsWith("/"))
                return SpliceRelativePath(sourceUri.Substring(0, sourceUri.Length - 1), relativePath);

            return $"{sourceUri}/{relativePath}";
        }

        public static string GetParent(string source)
        {
            int last_slash = source.LastIndexOf('/');
            if (last_slash < 0)
                return source;

            int previous_slash = source.LastIndexOf('/', last_slash - 1);
            if (previous_slash == last_slash - 1)
                return source;

            if (last_slash == source.Length - 1)
            {
                last_slash = source.LastIndexOf('/', last_slash - 1);
            }
            return source.Substring(0, last_slash);
        }

        public static string AddSchemeIfNot(string source)
        {
            return source.ToLower().StartsWith(_uiKit_UGUI_Scheme_Lower) ? source : $"{_uiKit_UGUI_Scheme}{source}";
        }

    }
#nullable restore
}
