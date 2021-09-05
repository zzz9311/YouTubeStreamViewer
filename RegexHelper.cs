using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YouTubeViewer
{
    class RegexHelper
    { //"Сейчас смотрят: (.+?)"
        public static Regex Viewers = new Regex(@"Сейчас смотрят: (.+?)", RegexOptions.Compiled);

        public static Regex Title = new Regex(@"\""title\"":{\""runs\"":\[{\""text\"":\""(.+?)\""}",RegexOptions.Compiled);

        public static Regex ViewUrl =new Regex(@"videostatsWatchtimeUrl\"":{\""baseUrl\"":\""(.+?)\""}", RegexOptions.Compiled);

        public static Regex Trash = new Regex(@"[=/\-+]", RegexOptions.Compiled);
    }
}
