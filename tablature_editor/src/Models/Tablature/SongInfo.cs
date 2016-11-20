using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFE.Models
{
    public class SongInfo
    {
        public string Artist = "artiste";
        public string Title = "title";
        public string Instrument = "instrument";

        public override string ToString()
        {
            return "Artist\t\t: " + Artist + "\r\n" +
                    "Title\t\t: " + Title + "\r\n" +
                    "Instrument\t: " + Instrument + "\r\n";
        }

        public static SongInfo FromStringList(List<string> strList)
        {
            SongInfo songInfo = new SongInfo();
            songInfo.Artist = strList[0].Split(new[] { ": " }, StringSplitOptions.None)[1].TrimEnd(new[] { '\r' });
            songInfo.Title = strList[1].Split(new[] { ": " }, StringSplitOptions.None)[1].TrimEnd(new[] { '\r' });
            songInfo.Instrument = strList[2].Split(new[] { ": " }, StringSplitOptions.None)[1].TrimEnd(new[] { '\r' });
            return songInfo;
        }
    }
}
