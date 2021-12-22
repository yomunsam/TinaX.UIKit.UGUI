﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Exceptions;
using TinaX.UIKit.Args;
using TinaX.UIKit.Page.Navigator;
using TinaX.UIKit.UGUI.Helper;
using TinaX.UIKit.UGUI.Services;

namespace TinaX.UIKit.UGUI.Page.Navigator
{
    public class UGUIPageNavigator : PageNavigator, IPageNavigator<UGUIPage, OpenUGUIArgs>
    {
        private readonly UGUIPage m_UGUIPage;
        private readonly IUIKitUGUI m_UIKitUGUI;

        public UGUIPageNavigator(UGUIPage page, IUIKit uikit, IUIKitUGUI uiKitUGUI) : base(page, uikit)
        {
            this.m_UGUIPage = page;
            this.m_UIKitUGUI = uiKitUGUI;
        }

        public virtual async UniTask<UGUIPage> OpenUIAsync(OpenUGUIArgs args, CancellationToken cancellationToken)
        {
            if (m_Page.Parent == null)
                throw new XException($"Page {m_Page.Name} must have parent");
            ProcessArgsBeforeGetPage(ref args);
            var page = await m_UIKitUGUI.GetUIPageAsync(args, cancellationToken);
            m_UGUIPage.ParentUGUI.Push(page, args.PushToGroupArgs);
            return page;
        }

        protected override void ProcessArgsBeforeGetPage(ref OpenUIArgs args)
        {
            if(!PageUriHelper.IsMatch(args.PageUri))
            {
                if (PageUriHelper.IsRelativePath(args.PageUri))
                    args.PageUri = PageUriHelper.SpliceRelativePath(PageUriHelper.GetParent(m_Page.PageUri), args.PageUri); //处理相对路径
            }
        }

        protected virtual void ProcessArgsBeforeGetPage(ref OpenUGUIArgs args)
        {
            if (!PageUriHelper.IsMatch(args.PageUri))
            {
                if (PageUriHelper.IsRelativePath(args.PageUri))
                    args.PageUri = PageUriHelper.SpliceRelativePath(PageUriHelper.GetParent(m_Page.PageUri), args.PageUri); //处理相对路径
            }
        }
    }
}
