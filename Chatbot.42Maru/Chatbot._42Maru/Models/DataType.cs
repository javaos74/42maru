using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot._42Maru.Models
{

    public class Button
    {
        public string label { get; set; }
        public string payload { get; set; }
        public string button_type { get; set; } = "link";
        public string link_mobile { get; set; }
        public string link_pc { get; set; }

        public Button()
        {

        }
        public Button( string lable, string payload, string mobile, string pc)
        {
            this.label = lable;
            this.payload = payload;
            this.link_mobile = mobile;
            this.link_pc = pc;
        }
    }

    public class MediaData
    {
        public string media_type { get; set; }
        public string media_url { get; set; }
    }
    public class MediaMessage
    {
        public string type { get; set; } = "output";
        public string data_type { get; set; } = "media";

        public MediaData data { get; set; } = new MediaData();

        public MediaMessage( MediaData d)
        {
            this.data = d;
        }
    }
    public class CarouselData {
        public string title { get; set; }
        public string text { get; set; }
        public string media_type { get; set; }
        public string media_url { get; set; }
        public string link_mobile { get; set; }
        public string link_pc { get; set; }

        public List<Button> buttons { get; set; } = new List<Button>();

    }
    public class CarouselMessage
    {
        public string type { get; set; } = "output";
        public string data_type { get; set; } = "carousel";

        public List<CarouselData> data { get; set; } = new List<CarouselData>();

        public CarouselMessage() { }
        public CarouselMessage( CarouselData d)
        {
            this.data.Add(d);
        }
    }
    public class TextData
    {
        public string text { get; set; }

        public TextData() {  }
        public TextData(string t)
        {
            this.text = t;
        }
    }
    public class TextMessage
    {
        public string type { get; set; } = "output";
        public string data_type { get; set; } = "text";
        public List<TextData> data { get; set; } = new List<TextData>();

        public TextMessage() { }
        public TextMessage( TextData t)
        {
            data.Add(t);
        }
    }
    public class TextReply
    {
        public string scenario_id { get; set; }
        public string session_id { get; set; }

        public List<TextMessage> messages { get; set; } = new List<TextMessage>();
    }

    public class MediaReply
    {
        public string scenario_id { get; set; }
        public string session_id { get; set; }
        public List<MediaMessage> messages { get; set; } = new List<MediaMessage>();
    }

    public class CarouselReply
    {
        public string scenario_id { get; set; }
        public string session_id { get; set; }

        public List<CarouselMessage> messages { get; set; } = new List<CarouselMessage>();
    }
}
