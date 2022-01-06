using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace EncryptionAssistant.banzhu
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class guanyu : Page
    {
        public guanyu()
        {
            this.InitializeComponent();
        }

        private async void Hyperlink_ClickAsync(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            // Windows.ApplicationModel.Email
            EmailMessage message = new EmailMessage();
            // Mail subject
            EmailRecipient emailRecipient = new EmailRecipient("LittleFishTenyears@outlook.com");
            message.To.Add(emailRecipient);

            await EmailManager.ShowComposeNewEmailAsync(message);
        }
    }
}
