using System;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.UGUI.Animation;
using TinaX.UIKit.UGUI.Page;
using TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage;

#nullable enable

namespace TinaX.UIKit.UGUI.GetUGuiUIPage.Handlers
{
    public class AddSimpleAnimationsAsyncHandler : IGetUGuiPageAsyncHandler
    {
        public string HandlerName => HandlerNameConsts.AddSimpleAnimation;

        public UniTask GetPageAsync(GetUGuiPageContext context, GetUGuiPagePayload payload, CancellationToken cancellationToken)
        {
            if (payload.UIPage == null)
                return UniTask.CompletedTask;

            //UI简单动画
            Type? controllerType = payload.UIPage!.Controller == null ? null : context.Services.XCore.GetObjectType(payload.UIPage.Controller);
            

            //显示动画
            string? displayedAnimationName = payload.Args.DisplayedSimpleAnimationName;
            if(displayedAnimationName == null && controllerType != null)
            {
                var attribute = controllerType.GetCustomAttribute<DisplayedSimpleAnimationAttribute>();
                if (attribute != null)
                    displayedAnimationName = attribute.AnimationName;
            }

            if (displayedAnimationName != null)
            {
                if (context.UIKitUGUI.SimpleAnimationManager.TryGet(displayedAnimationName!, out var playDelegate))
                {
                    payload.UIPage.OnPageDisplayed(page =>
                    {
                        if (page is UGUIPage uguiPage && uguiPage.Transform != null)
                        {
                            playDelegate(uguiPage.Transform);
                        }
                    });
                }
            }



            //关闭动画
            string? closedSimpleAnimationName = payload.Args.ClosedSimpleAnimationName;
            if(closedSimpleAnimationName == null && controllerType != null)
            {
                var attribute = controllerType.GetCustomAttribute<ClosedSimpleAnimationAttribute>();
                if (attribute != null)
                    closedSimpleAnimationName = attribute.AnimationName;
            }
            if (closedSimpleAnimationName != null)
            {
                if (context.UIKitUGUI.SimpleAnimationManager.TryGet(closedSimpleAnimationName!, out var playDelegate))
                {
                    payload.UIPage.OnPageClosed(page =>
                    {
                        if (page is UGUIPage uguiPage && uguiPage.Transform != null)
                        {
                            var delay = playDelegate(uguiPage.Transform);
                            uguiPage.SetCloseDelayTime(delay);
                        }
                    });
                }
            }

            return UniTask.CompletedTask;
        }
    }

}
