using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using Chatbot._42Maru.Activities.Design.Designers;
using Chatbot._42Maru.Activities.Design.Properties;

namespace Chatbot._42Maru.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(SendText), categoryAttribute);
            builder.AddCustomAttributes(typeof(SendText), new DesignerAttribute(typeof(SendTextDesigner)));
            builder.AddCustomAttributes(typeof(SendText), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(SendMedia), categoryAttribute);
            builder.AddCustomAttributes(typeof(SendMedia), new DesignerAttribute(typeof(SendMediaDesigner)));
            builder.AddCustomAttributes(typeof(SendMedia), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(SendCarousel), categoryAttribute);
            builder.AddCustomAttributes(typeof(SendCarousel), new DesignerAttribute(typeof(SendCarouselDesigner)));
            builder.AddCustomAttributes(typeof(SendCarousel), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
