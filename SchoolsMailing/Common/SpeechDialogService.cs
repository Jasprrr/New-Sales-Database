using SchoolsMailing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsMailing.Common
{
    public interface ISpeechDialogService
    {
        Task ShowAsync();
    }

    public class SpeechDialogService : ISpeechDialogService
    {
        public async Task ShowAsync()
        {
            var contentDialog = new SpeechDialogService();
            await contentDialog.ShowAsync();

        }
    }
}
